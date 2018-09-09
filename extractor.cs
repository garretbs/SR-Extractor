
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
// using System.ComponentModel;
// using System.Drawing;
// using System.Windows.Forms;


/*
public class SR_Form : Form{

	public SR_Form(){
		this.AllowDrop = true;
	}

	public static void Main(){
		Application.EnableVisualStyles();
		Application.Run(new SR_Form());
	}
}
*/

public enum Formats {None, WAV, ICO, PNG, Other};

public class SR_SubFile{
	public UInt32 offset;
	public UInt32 size;
	public UInt32 channels;
	public UInt32 bitrate;
	public UInt16 precision;
	public string filename;
	public int format;

	public SR_SubFile(string[] headerInfo){
		//problem: first element is sometimes ""
		int i = 0;
		if(headerInfo[0] == "") i++;
		this.offset = UInt32.Parse(headerInfo[i++]);
		this.size = UInt32.Parse(headerInfo[i++]);
		this.channels = UInt32.Parse(headerInfo[i++]);
		this.bitrate = UInt32.Parse(headerInfo[i++]);
		this.precision = UInt16.Parse(headerInfo[i++]);
		this.filename = headerInfo[i];
		string[] filenameData = Regex.Split(headerInfo[i], @"\.");
		if(filenameData.Length < 2){
			this.format = (int) Formats.None;
		}else{
			switch(filenameData[1].ToUpper()){
				case "WAV":
					this.format = (int) Formats.WAV;
					break;
				case "ICO":
					this.format = (int) Formats.ICO;
					break;
				default:
					this.format = (int) Formats.Other;
					break;
			}
		}
	}

	public void Output(FileStream headerFile, string directory){
		System.Console.WriteLine("Extracting " + this.filename + "...");
		FileStream file = new FileStream(directory+this.filename, FileMode.CreateNew);
		int bytesToRead = (int) this.size;
		byte[] buffer = new byte[1024];
		int bytesRead = 0;
		int totalBytesRead = 0;
		
		
		switch(this.format){
			case (int) Formats.WAV:
				//for now, assumethe system is little endian
				// #http://www.topherlee.com/software/pcm-tut-wavformat.html
				// #http://www.signalogic.com/index.pl?page=ms_waveform			
				file.Write(Encoding.ASCII.GetBytes("RIFF"), 0, 4); //'RIFF' section tag (4 bytes)
				file.Write(System.BitConverter.GetBytes((UInt32) (@size+44-8)), 0, 4); //length of RIFF section (4 bytes)
				file.Write(Encoding.ASCII.GetBytes("WAVE"), 0, 4); //wave format identifier (4 bytes)
				file.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4); //'fmt' section tag (4 bytes)
				file.Write(System.BitConverter.GetBytes((UInt32) 16), 0, 4); //length of 'fmt' section (4 bytes)
				file.Write(System.BitConverter.GetBytes((UInt16) 1), 0, 2); //wave type (1 for PCM) (2 bytes)
				file.Write(System.BitConverter.GetBytes((UInt16) this.channels), 0, 2); //number of channels (2 bytes)
				file.Write(System.BitConverter.GetBytes((UInt32) this.bitrate), 0, 4); //sample rate (4 bytes)
				file.Write(System.BitConverter.GetBytes(getBytesPerSecond()), 0, 4); //bytes per second (4 bytes)
				file.Write(System.BitConverter.GetBytes(getBlockAlignment()), 0, 2); //block alignment (2 bytes)
				file.Write(System.BitConverter.GetBytes(this.precision), 0, 2); //precision (2 bytes)
				file.Write(Encoding.ASCII.GetBytes("data"), 0, 4); //'data' section tag (4 bytes)
				file.Write(System.BitConverter.GetBytes(this.size), 0, 4); //size of data section tag (4 bytes)
				break;
			// case Formats.ICO:
				// break;
			// case (int) Formats.PNG:
				// System.Console.WriteLine(headerFile.Position);
				// break;
		}
		//all files raw data
		while(bytesToRead > 0){
			//System.Console.WriteLine(bytesToRead);
			bytesRead = headerFile.Read(buffer, 0, Math.Min(bytesToRead, buffer.Length));
			bytesToRead -= bytesRead;
			totalBytesRead += bytesRead;
			//if(bytesToRead < 0) System.Console.WriteLine(bytesToRead);
			file.Write(buffer, 0, bytesRead);
		}
		
		//System.Console.Write(headerFile.Position);
		//System.Console.Write(", ");
		//System.Console.WriteLine(headerFile.Position-(2048+totalBytesRead));
		//writer.Close();
		file.Close();
	}
	
	private UInt32 getBytesPerSecond(){
		return ((this.channels*this.bitrate*this.precision)>>3);
	}
	
	private UInt16 getBlockAlignment(){
		return (UInt16) ((this.channels*this.precision)>>3);
	}
}

public class SR_Header{
	public int numFiles;
	public int always80;
	public int rawOffset;
	
	public SR_Header(){
	}
	
	public void SetInfo(string[] headerInfo){
		this.numFiles = Int32.Parse(headerInfo[0]);
		this.always80 = Int32.Parse(headerInfo[1]);
		this.rawOffset = Int32.Parse(headerInfo[2]);
	}
}

public class SR_Extractor : Form{
	
	private Label filename;
	private string selectedArchive;
	
	public SR_Extractor(){
		Text = "SR Archive Extractor";
		Size = new Size(480, 360);
		CenterToScreen();
		
		int baseY = 24;
		selectedArchive = "";
		
		
		MainMenu menu = new MainMenu();
		menu.MenuItems.Add(new MenuItem("Open File", new EventHandler(this.openFile), Shortcut.CtrlO));
		//file.MenuItems.Add();
		Menu = menu;
		
		
		filename = new Label();
		filename.Text = "(No file selected)";
		filename.Size = new Size(128, 24);
		filename.Location = new Point(0, baseY>>1);
		filename.Parent = this;
		
		/*
		TextBox srFileList = new TextBox();
		srFileList.Parent = this;
		srFileList.Location = new Point(0, baseY);
		srFileList.ReadOnly = true;
		*/
		
		Button extractButton = new Button();
		extractButton.Text = "Extract archive";
		extractButton.Size = new Size(128, 24);
		extractButton.Location = new Point(0, baseY*2);
		//extractButton.Anchor = AnchorStyles.Left;
		extractButton.Click += new EventHandler(extractClick);
		extractButton.Parent = this;
		Controls.Add(extractButton);
		
		Button quitButton = new Button();
		quitButton.Text = "Quit";
		quitButton.Size = new Size(64, 24);
		quitButton.Location = new Point(0, baseY*3);
		//quitButton.Anchor = AnchorStyles.Left;
		quitButton.Click += new EventHandler(quitClick);
		quitButton.Parent = this;
		Controls.Add(quitButton);
	}
	
	public void openFile(object sender, EventArgs e){
		OpenFileDialog dialog = new OpenFileDialog();
		dialog.ShowHelp = true; //necessary or it hangs
		dialog.Filter = "Sega aRchive files (*.sr) | *.sr";
		
		if(dialog.ShowDialog(this) == DialogResult.OK){
			filename.Text = dialog.FileName;
			selectedArchive = dialog.FileName;
			Console.WriteLine(dialog.FileName);
			//StreamReader reader = new StreamReader(dialog.FileName);
			//reader.Close();
		}
	}

	public void extractClick(object sender, EventArgs e){
		//check if archive selected
		if(selectedArchive != ""){
			//Console.WriteLine(selectedArchive);
			int lineNum = 0;
			bool gotHeader = false;
			SR_Header srHeader = new SR_Header();
			string[] headerInfo;
			List<SR_SubFile> subFiles = new List<SR_SubFile>();
			
			if(!File.Exists(selectedArchive)){
				System.Console.WriteLine("Could not find file "+selectedArchive);
				return;
			}
			
			string dir = selectedArchive+ " output\\";
			
			if(Directory.Exists(dir)){Directory.Delete(dir, true);}
			Directory.CreateDirectory(dir);

			foreach(var line in File.ReadLines(selectedArchive)){
				//System.Console.WriteLine(line);
				lineNum++;
				if(!gotHeader){
					headerInfo = Regex.Split(line, @"\s+");
					//System.Console.WriteLine(string.Join(",", headerInfo));
					srHeader.SetInfo(headerInfo);
					gotHeader = true;
				}else{
					headerInfo = Regex.Split(line, @"\s+");
					//System.Console.WriteLine(string.Join(",", headerInfo));
					subFiles.Add(new SR_SubFile(headerInfo));
					if(lineNum > srHeader.numFiles) break;
				}
			}
			
			FileStream headerFile = new FileStream(selectedArchive, FileMode.Open, FileAccess.Read);
			//headerFile.Seek(srHeader.rawOffset, SeekOrigin.Begin);
			foreach(var srFile in subFiles){
				//System.Console.WriteLine(srFile.filename);
				headerFile.Seek(srHeader.rawOffset+srFile.offset, SeekOrigin.Begin); //shouldn't be necessary
				srFile.Output(headerFile, dir);
				
			}
			headerFile.Close();
			Console.WriteLine("Done!");
			System.Media.SystemSounds.Beep.Play();
		}else{
			System.Media.SystemSounds.Beep.Play();
		}
	}
	
	public void quitClick(object sender, EventArgs e){
		Close();
	}

	public static void Main(string[] args){
		Application.Run(new SR_Extractor());
	}
}

//todo: turn into gui
//have custom icon for funsies?