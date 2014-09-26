//Controls the bike state machine, and reads speed and heart rate from the bike

using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Text;

public class BikeController : MonoBehaviour {
	
	//These three variables are what the game cares about as input from the bike
	public float speed = 0.0f;		//1.0f at 60rpm
	public int RPM = 0;
	public int heartRate = 0;
	
	public int status = -1;
	
	//This variable exists to allow for the enabling and disabling of the bike via the editor
	public bool enableBike = true;
	public bool bikePresent = false;
	public string portName = "COM4";
	public bool ending = false;					//This is used as a primary signal as part of the shutdown process to make sure that
	//the port is properly closed when the user stops playing. When the user gets a game over, this gets
	//set to true. The thread that manages the bike will periodically check this value, and shut down if it is set to true
	
	private string infoTextString = string.Empty;
	private Thread sideThread;
	private SerialPort port;
	private byte[] buffer = new byte[1024];
	private int bufferIndex = 0;
	
	//Commands for the bike. Some of these are not supported by the 95CI bike.
	private static byte[] EMPTY_FRAME = {0xF1, 0x00, 0xF2};
	private static byte[] UP_LIST = {0xF1, 0x80, 0x02, 0x02, 0xA5, 0xB0, 0x95, 0xF2};	//tells the bike that the batch data for upload should include speed and heart rate
	private static byte[] GET_UP_LIST = {0xF1, 0xAA, 0xAA, 0xF2}; //requests upload of batch data
	//private static byte[] GO_IDLE = {0xF1, 0x80, 0x82, 0x02, 0xF2};
	//private static byte[] GO_HAVEID = {0xF1, 0x80, 0x83, 0x03, 0xF2};
	//private static byte[] GO_INUSE = {0xF1, 0x80, 0x85, 0x05, 0xF2};
	//private static byte[] GET_SPEED = {0xF1, 0xA5, 0xA5, 0xF2};
	
	//Initialises the monobehavior
	void Start () {
		ReinitialiseBike ();
	}

	//Initialises the bike thread
	public void ReinitialiseBike() {
		status = -1;
		ending = false;
		if (enableBike) {
			sideThread = new Thread (SideThreadStart);
			sideThread.Start ();
		}
	}
	
	//The method to be used for the separate thread that reads the serial port. We're using a side thread because Mono's implementation of Serial is bugged
	//when it comes to checking if the port has data, so we need the thread to sit there listening for it.
	void SideThreadStart() {
		//Debug.Log ("Opening Port");
		port = new SerialPort (portName, 9600, Parity.None, 8, StopBits.One);
		try {
			port.Open();
			port.ReadTimeout = 10000;
			Debug.Log ("Bike Port Open");
			bikePresent = true;
			sendThenRead(EMPTY_FRAME);	//We send the empty frame at the start because the response to it should be the same as what the bike sends when first turned on
			//Thus when the game starts, we should get to the same point whether the bike was turned on before the game begins or not.
			sendThenRead(UP_LIST);		//Sets up the batch data upload
			//sendThenRead (GO_IDLE);		//These three status commands transition the bike from the ready state to the inUse state.
			//sendThenRead (GO_HAVEID);
			//sendThenRead (GO_INUSE);
			Debug.Log ("Finished setup");
			FetchDataLoop();

		} catch (IOException e) {
			Debug.Log (e.ToString());
			bikePresent = false;
		} finally {
			//When the thread finishes, we close the port
			Debug.Log("Closing Port");
			if (port != null && port.IsOpen) {
				port.Close();
			}
			Debug.Log ("Port Closed");
		}
	}
	
	//Loops constantly, requesting batch data from the bike, waiting till the data arrives, then processing the data
	void FetchDataLoop() {
		while(!ending) {
			sendCommand(GET_UP_LIST);
			readUntilFrameRecieved();
		}
	}
	
	//sends a command to the bike
	void sendCommand(byte[] command) {
		port.Write (command, 0, command.Length);
	}
	
	//Sends a command to the bike, then waits for a response and processes it
	void sendThenRead(byte[] command) {
		sendCommand(command);
		readUntilFrameRecieved ();
	}
	
	//Reads data from the port until a frame stop flag is recieved
	void readUntilFrameRecieved() {
		int readValue = port.ReadByte ();
		bool frameRead = false;
		while (!frameRead) {
			buffer[bufferIndex] = (byte)readValue;
			bufferIndex++;
			if (readValue == 0xF2) {
				frameRead = true;
			} else {
				readValue = port.ReadByte ();
			}
		}
		Debug.Log ("Full Frame Recieved: " + convertBufferToString () + " " + infoTextString);
		InterpretFrame ();
		infoTextString = createDataString();
		bufferIndex = 0;
		
	}
	
	//Checks that the frame has a valid checksum, and if it contains data structures. If so, the data structures are sent off to be processed
	void InterpretFrame() {
		//First, we check that the frame checksum is good
		int actualChecksum = buffer[bufferIndex - 2];
		int expectedChecksum = 0;
		for (int i = 1; i < bufferIndex - 2; i++) {
			expectedChecksum = expectedChecksum ^ buffer[i];
		}
		if (actualChecksum != expectedChecksum) {
			//Debug.LogWarning ("Bad checksum");
		} else {
			//Checksum was good, update the status
			status = buffer[1];
			//Checksum was good, if this frame contained more than just status byte + checksum, divide it into data structures and process them
			if (bufferIndex > 4) {
				ProcessDataStructures();
			}
		}
	}
	
	//Looks at the data in the buffer, picks out the data structures, splits them, and sends them to be interpreted
	void ProcessDataStructures() {
		Debug.Log ("Processing Data structures");
		int dataBytesRemaining = bufferIndex - 4; //start, status, checksum, stop
		int currentDataStructureStartPoint = 2; //skip start and status	
		bool dataStructuresRemaining = true;
		while (dataStructuresRemaining) {
			int currentDataStructureSize = 2 + buffer[currentDataStructureStartPoint + 1];	//identifier + databyteCountByte + databyteCount
			Debug.Log(string.Format("CDSS: {0}, CDSSP: {1}", currentDataStructureSize, currentDataStructureStartPoint));
			int[] newDataStructure = new int[currentDataStructureSize];
			int index = 0;
			//copy the data structure from the buffer to the data structure holder
			for (int i = currentDataStructureStartPoint; i < currentDataStructureStartPoint + currentDataStructureSize; i++) {
				newDataStructure[index] = buffer[i];
				index++;
			}
			//process it
			ProcessDataStructure(newDataStructure);
			//determine if there is another data structure left, if so move the start point to where it starts
			dataBytesRemaining -= currentDataStructureSize;
			if (dataBytesRemaining > 0) {
				currentDataStructureStartPoint = currentDataStructureStartPoint + currentDataStructureSize;
			} else {
				dataStructuresRemaining = false;
			}
		}
	}
	
	//Takes an individual data structure and interprets it
	void ProcessDataStructure(int[] dataStructure) {
		BikeIdentifier identifier = (BikeIdentifier)(dataStructure[0]);
		switch (identifier) {
		case BikeIdentifier.CMDGetSpeed:
			//we should have 3 data bytes after the identifier & data bytes count
			//we expect the relevant info to be in the first of these 3 bytes, a rpm  value
			RPM = dataStructure[2];
			speed = (float)dataStructure[2] / 60.0f;
			break;
		case BikeIdentifier.CMDGetHRCur:
			//we should have only a single data byte after the identifier & data bytes count
			heartRate = dataStructure[2];
			break;
		default:
			Debug.Log("Unrecognised data");
			break;
		}
	}
	
	//Formats the contents of the buffer in hex as a string
	string convertBufferToString() {
		StringBuilder builder = new StringBuilder ();
		for (int i = 0; i < bufferIndex; i++) {
			string part = buffer[i].ToString("X");
			if (part.Length == 1) {
				builder.Append("0");
			}
			builder.Append(part);
			builder.Append(' ');
		}
		return builder.ToString ();
	}
	
	//returns a string containing the bike statistics
	string createDataString() {
		return string.Format("Speed: {0}, Heart Rate: {1}", speed, heartRate);	
	}
}

public enum BikeIdentifier : int {
	CMDGetSpeed = 0xA5,
	CMDGetHRCur = 0xB0
}