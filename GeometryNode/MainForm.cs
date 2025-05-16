 
using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

namespace GeometryNode
{
	/// <summary>
	/// Description of Form2.
	/// </summary>
	public partial class MainForm : Form
	{
		
		// Import the necessary functions from user32.dll
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

		[DllImport("kernel32.dll")]
		static extern uint GetCurrentThreadId();

		[DllImport("user32.dll")]
		static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
		static void BringAppToForeground(IntPtr hWnd)
		{
			// Get the ID of the thread that created the window.
			int targetThreadId;
			GetWindowThreadProcessId(hWnd, out targetThreadId);
			uint currentThreadId = GetCurrentThreadId();

			// Attach the current thread to the thread of the window to be activated.  This is often necessary
			// to bypass restrictions that prevent a background process from stealing focus.
			if (currentThreadId != targetThreadId) {
				AttachThreadInput(currentThreadId, (uint)targetThreadId, true);
			}

			// Try to set the foreground window.
			bool result = SetForegroundWindow(hWnd);

			// Detach the current thread from the other thread.  This is important to maintain system stability.
			if (currentThreadId != targetThreadId) {
				AttachThreadInput(currentThreadId, (uint)targetThreadId, false);
			}

			if (!result) {
				// If SetForegroundWindow fails, it might be due to restrictions in newer Windows versions.
				// Consider alternative methods if necessary, but they may require more privileges or
				// might not work reliably in all situations.  For example, you could try sending
				// window messages, but that's beyond the scope of this basic example.
				Console.WriteLine("SetForegroundWindow failed. The application may be running with higher privileges.");
			}
		}
		
		SQLiteConnection conn;
		public MainForm()
		{
			
			InitializeComponent();
			
			conn = new SQLiteConnection(new SQLiteConnectionStringBuilder {
				DataSource = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "GeometryNode.db"),
				JournalMode = SQLiteJournalModeEnum.Truncate
			}.ConnectionString);
			conn.Open();
		
			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
				cmd.CommandText = @"create table if not exists GeometryNode(Id integer not null primary key autoincrement, Name text not null unique, Title text not null unique,Views integer DEFAULT 0,CreateAt datetime default (datetime('now','localtime')),UpdateAt datetime default (datetime('now','localtime')));";
				cmd.ExecuteNonQuery();
			}
//			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
//				cmd.CommandText = @"insert into GeometryNode(Title,Name) values(@Title,@Name)";
//					//var items =new []{"FunctionNodeCompare","FunctionNodeRandomValue","FunctionNodeAlignEulerToVector","FunctionNodeRotateEuler","FunctionNodeAlignRotationToVector","FunctionNodeAxesToRotation","FunctionNodeAxisAngleToRotation","FunctionNodeEulerToRotation","FunctionNodeInvertRotation","FunctionNodeRotateRotation","FunctionNodeRotateVector","FunctionNodeRotationToAxisAngle","FunctionNodeRotationToEuler","FunctionNodeRotationToQuaternion","FunctionNodeQuaternionToRotation","FunctionNodeCombineMatrix","FunctionNodeCombineTransform","FunctionNodeMatrixDeterminant","FunctionNodeInvertMatrix","FunctionNodeMatrixMultiply","FunctionNodeProjectPoint","FunctionNodeSeparateMatrix","FunctionNodeSeparateTransform","FunctionNodeTransformDirection","FunctionNodeTransformPoint","FunctionNodeTransposeMatrix","FunctionNodeBooleanMath","FunctionNodeIntegerMath","FunctionNodeFloatToInt","FunctionNodeHashValue","FunctionNodeReplaceString","FunctionNodeSliceString","FunctionNodeStringLength","FunctionNodeFindInString","FunctionNodeValueToString","FunctionNodeInputSpecialCharacters","FunctionNodeCombineColor","FunctionNodeSeparateColor","FunctionNodeInputBool","FunctionNodeInputColor","FunctionNodeInputInt","FunctionNodeInputRotation","FunctionNodeInputString","FunctionNodeInputVector"};
//				
//				//var items =new string[]{ "GeometryNodeStoreNamedAttribute", "GeometryNodeCurvePrimitiveLine", "GeometryNodeInputIndex", "GeometryNodeGroup", "GeometryNodeIndexSwitch", "GeometryNodeMenuSwitch", "GeometryNodeSwitch", "GeometryNodeAccumulateField", "GeometryNodeFieldAtIndex", "GeometryNodeFieldOnDomain", "GeometryNodeStringJoin", "GeometryNodeStringToCurves", "GeometryNodeImageTexture", "GeometryNodeReplaceMaterial", "GeometryNodeInputMaterialIndex", "GeometryNodeMaterialSelection", "GeometryNodeSetMaterial", "GeometryNodeSetMaterialIndex", "GeometryNodeVolumeCube", "GeometryNodeVolumeToMesh", "GeometryNodeDistributePointsInVolume", "GeometryNodeDistributePointsOnFaces", "GeometryNodePoints", "GeometryNodePointsToCurves", "GeometryNodePointsToVertices", "GeometryNodePointsToVolume", "GeometryNodeSetPointRadius", "GeometryNodeUVPackIslands", "GeometryNodeUVUnwrap", "GeometryNodeCornersOfEdge", "GeometryNodeCornersOfFace", "GeometryNodeCornersOfVertex", "GeometryNodeEdgesOfCorner", "GeometryNodeEdgesOfVertex", "GeometryNodeFaceOfCorner", "GeometryNodeOffsetCornerInFace", "GeometryNodeVertexOfCorner", "GeometryNodeMeshCone", "GeometryNodeMeshCube", "GeometryNodeMeshCylinder", "GeometryNodeMeshGrid", "GeometryNodeMeshIcoSphere", "GeometryNodeMeshCircle", "GeometryNodeMeshLine", "GeometryNodeMeshUVSphere", "GeometryNodeDualMesh", "GeometryNodeEdgePathsToCurves", "GeometryNodeEdgePathsToSelection", "GeometryNodeExtrudeMesh", "GeometryNodeFlipFaces", "GeometryNodeMeshBoolean", "GeometryNodeMeshToCurve", "GeometryNodeMeshToPoints", "GeometryNodeMeshToVolume", "GeometryNodeScaleElements", "GeometryNodeSplitEdges", "GeometryNodeSubdivideMesh", "GeometryNodeSubdivisionSurface", "GeometryNodeTriangulate", "GeometryNodeSetShadeSmooth", "GeometryNodeSampleNearestSurface", "GeometryNodeSampleUVSurface", "GeometryNodeInputMeshEdgeAngle", "GeometryNodeInputMeshEdgeNeighbors", "GeometryNodeInputMeshEdgeVertices", "GeometryNodeEdgesToFaceGroups", "GeometryNodeInputMeshFaceArea", "GeometryNodeMeshFaceSetBoundaries", "GeometryNodeInputMeshFaceNeighbors", "GeometryNodeInputMeshFaceIsPlanar", "GeometryNodeInputShadeSmooth", "GeometryNodeInputEdgeSmooth", "GeometryNodeInputMeshIsland", "GeometryNodeInputShortestEdgePaths", "GeometryNodeInputMeshVertexNeighbors", "GeometryNodeInstanceOnPoints", "GeometryNodeInstancesToPoints", "GeometryNodeRealizeInstances", "GeometryNodeRotateInstances", "GeometryNodeScaleInstances", "GeometryNodeTranslateInstances", "GeometryNodeSetInstanceTransform", "GeometryNodeInstanceTransform", "GeometryNodeInputInstanceRotation", "GeometryNodeInputInstanceScale", "GeometryNodeCurveOfPoint", "GeometryNodeOffsetPointInCurve", "GeometryNodePointsOfCurve", "GeometryNodeCurveArc", "GeometryNodeCurvePrimitiveBezierSegment", "GeometryNodeCurvePrimitiveCircle", "GeometryNodeCurveSpiral", "GeometryNodeCurveQuadraticBezier", "GeometryNodeCurvePrimitiveQuadrilateral", "GeometryNodeCurveStar", "GeometryNodeCurvesToGreasePencil", "GeometryNodeCurveToMesh", "GeometryNodeCurveToPoints", "GeometryNodeDeformCurvesOnSurface", "GeometryNodeFillCurve", "GeometryNodeFilletCurve", "GeometryNodeGreasePencilToCurves", "GeometryNodeInterpolateCurves", "GeometryNodeMergeLayers", "GeometryNodeResampleCurve", "GeometryNodeReverseCurve", "GeometryNodeSubdivideCurve", "GeometryNodeTrimCurve", "GeometryNodeSetCurveNormal", "GeometryNodeSetCurveRadius", "GeometryNodeSetCurveTilt", "GeometryNodeSetCurveHandlePositions", "GeometryNodeCurveSetHandles", "GeometryNodeSetSplineCyclic", "GeometryNodeSetSplineResolution", "GeometryNodeCurveSplineType", "GeometryNodeSampleCurve", "GeometryNodeInputCurveHandlePositions", "GeometryNodeCurveLength", "GeometryNodeInputTangent", "GeometryNodeInputCurveTilt", "GeometryNodeCurveEndpointSelection", "GeometryNodeCurveHandleTypeSelection", "GeometryNodeInputSplineCyclic", "GeometryNodeSplineLength", "GeometryNodeSplineParameter", "GeometryNodeInputSplineResolution", "GeometryNodeGeometryToInstance", "GeometryNodeJoinGeometry", "GeometryNodeBake", "GeometryNodeBoundBox", "GeometryNodeConvexHull", "GeometryNodeDeleteGeometry", "GeometryNodeDuplicateElements", "GeometryNodeMergeByDistance", "GeometryNodeSortElements", "GeometryNodeTransform", "GeometryNodeSeparateComponents", "GeometryNodeSeparateGeometry", "GeometryNodeSplitToInstances", "GeometryNodeSetGeometryName", "GeometryNodeSetID", "GeometryNodeSetPosition", "GeometryNodeProximity", "GeometryNodeIndexOfNearest", "GeometryNodeRaycast", "GeometryNodeSampleIndex", "GeometryNodeSampleNearest", "GeometryNodeInputID", "GeometryNodeInputNamedAttribute", "GeometryNodeInputNormal", "GeometryNodeInputPosition", "GeometryNodeInputRadius", "GeometryNodeViewer", "GeometryNodeWarning", "GeometryNodeInputActiveCamera", "GeometryNodeCollectionInfo", "GeometryNodeImageInfo", "GeometryNodeIsViewport", "GeometryNodeInputNamedLayerSelection", "GeometryNodeObjectInfo", "GeometryNodeInputSceneTime", "GeometryNodeSelfObject", "GeometryNodeGizmoDial", "GeometryNodeGizmoLinear", "GeometryNodeGizmoTransform", "GeometryNodeInputCollection", "GeometryNodeInputImage", "GeometryNodeInputMaterial", "GeometryNodeInputObject", "GeometryNodeAttributeStatistic", "GeometryNodeAttributeDomainSize", "GeometryNodeBlurAttribute", "GeometryNodeCaptureAttribute", "GeometryNodeRemoveAttribute", "GeometryNodeRepeatInput", "GeometryNodeRepeatOutput"};
//				 var items =new []{ "ShaderNodeCombineXYZ","ShaderNodeClamp","ShaderNodeFloatCurve","ShaderNodeMapRange","ShaderNodeMath","ShaderNodeMix","ShaderNodeVectorCurve","ShaderNodeVectorMath","ShaderNodeVectorRotate","ShaderNodeSeparateXYZ","ShaderNodeBlackbody","ShaderNodeValToRGB","ShaderNodeRGBCurve","ShaderNodeTexBrick","ShaderNodeTexChecker","ShaderNodeTexGabor","ShaderNodeTexGradient","ShaderNodeTexMagic","ShaderNodeTexNoise","ShaderNodeTexVoronoi","ShaderNodeTexWave","ShaderNodeTexWhiteNoise","ShaderNodeValue"};
//				
//				var prefix="ShaderNode";
//				
//				foreach (var element in items) {
//				
//					cmd.Parameters.Add("Title", DbType.String).Value = element.StartsWith(prefix) ?
//						element.Substring(prefix.Length) : element;
//					cmd.Parameters.Add("Name", DbType.String).Value = element;
//					cmd.ExecuteNonQuery();
//				}
//			}
			LoadData();
			listBox2.Items.Clear();
			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
				cmd.CommandText = @"select Title,Name from GeometryNode Order By Views DESC LIMIT 20";
				//cmd.Parameters.Add("q", DbType.String).Value = comboBox1.Text.Trim();
			
				using (var reader = cmd.ExecuteReader()) {
					while (reader.Read()) {
						listBox2.Items.Add(reader.GetString(0));
					
					}
				}
			}
			RegisterHotKey(this.Handle, (int)Keys.F5, 0, (int)Keys.F5);
			RegisterHotKey(this.Handle, (int)Keys.F4, 0, (int)Keys.F4);
		}

		IntPtr[] _processes;
		int _i;
		void SwitchBlender()
		{
			if(_processes==null)
				_processes=Process.GetProcessesByName("blender")
					.Select(x=>x.MainWindowHandle).ToArray();
			if(_i>1)
				_i=0;
			BringAppToForeground(_processes[_i++]);
			
			
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x0312) {
				/*
				  ushort id = (ushort)m.WParam;
        Keys key = (Keys)( ( (int)m.LParam >> 16 ) & 0xFFFF );
        Modifiers mods = (Modifiers)( (int)m.LParam & 0xFFFF );*/
				ushort id = (ushort)m.WParam;
				if (id == (ushort)Keys.F5) {
					//ShiftA();
					SwitchBlender();
				} else if (id == (ushort)Keys.F4) {
					//ShiftA();
				ShiftA();
				} 
				
				
			}
			base.WndProc(ref m);
		}
		void ListBox1DoubleClick(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1) {
				//LoadData();
			
			} else {
				using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
					cmd.CommandText = @"select Name,Title from GeometryNode where Title = @Title";
					cmd.Parameters.Add("Title", DbType.String).Value = listBox1.SelectedItem.ToString();
					using (var reader = cmd.ExecuteReader()) {
						if (reader.Read())
							Clipboard.SetText(reader.GetString(1));
					}
				}
				using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
					cmd.CommandText = @"update GeometryNode set Views = Views + 1,UpdateAt = (datetime('now','localtime')) where Title = @Title";
					cmd.Parameters.Add("Title", DbType.String).Value = listBox1.SelectedItem.ToString();
					using (var reader = cmd.ExecuteReader()) {
						if (reader.Read())
							Clipboard.SetText(reader.GetString(0));
					}
				}
			}
		}
		void LoadData()
		{

			if (comboBox1.Text.Length > 0) {
				listBox1.Items.Clear();
				using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
					cmd.CommandText = @"select Title,Name from GeometryNode Order By Views DESC";
					//cmd.Parameters.Add("q", DbType.String).Value = comboBox1.Text.Trim();
					var regex = new Regex(comboBox1.Text.Trim());
					using (var reader = cmd.ExecuteReader()) {
						while (reader.Read()) {
							if (regex.IsMatch(reader.GetString(0)) || regex.IsMatch(reader.GetString(1))) {
								listBox1.Items.Add(reader.GetString(0));
							}
						}
					}
				}
				return;
			}
			listBox1.Items.Clear();
			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
				cmd.CommandText = @"select Title from GeometryNode Order By Views DESC";
			
				using (var reader = cmd.ExecuteReader()) {
					while (reader.Read())
						listBox1.Items.Add(reader.GetString(0));
				}
			}
		}
		void ComboBox1KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) {
				listBox1.Items.Clear();
				using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
					cmd.CommandText = @"select Title,Name from GeometryNode Order By Views DESC";
					//cmd.Parameters.Add("q", DbType.String).Value = comboBox1.Text.Trim();
					var regex = new Regex(comboBox1.Text.Trim(), RegexOptions.IgnoreCase);
					using (var reader = cmd.ExecuteReader()) {
						while (reader.Read()) {
							var x = reader.GetString(0);
							if (regex.IsMatch(reader.GetString(0)) || regex.IsMatch(reader.GetString(1))) {
								listBox1.Items.Add(reader.GetString(0));
							}
						}
					}
				}
			}
		}
		
		void ShiftA()
		{
			keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
			Thread.Sleep(20);
			keybd_event((int)VK.A, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press
			Thread.Sleep(20);
			keybd_event((int)VK.A, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
			Thread.Sleep(20);
			keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release
			Thread.Sleep(20);
//			keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
//			Thread.Sleep(20);
//			keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
			
//			keybd_event((int)Keys.Space, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press
//			Thread.Sleep(20);
//			keybd_event((int)Keys.Space, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
//			Thread.Sleep(20);
//			keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press
//			Thread.Sleep(20);
//			keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press
//			Thread.Sleep(20);
//			keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
//			Thread.Sleep(20);
//			keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release
//		
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
	
		[DllImport("user32.dll")]
		static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
			uint wMsgFilterMax);
		[DllImport("user32.dll")]
		static extern bool TranslateMessage([In] ref MSG lpMsg);
		[DllImport("user32.dll")]
		static extern IntPtr DispatchMessage([In] ref MSG lpmsg);
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;

			public POINT(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}
       
		}
		[StructLayout(LayoutKind.Sequential)]

		public struct MSG
		{
			IntPtr hwnd;
			public	uint message;
			public	UIntPtr wParam;
			IntPtr lParam;
			int time;
			POINT pt;
			int lPrivate;
		}
		#pragma warning disable 649
		internal struct INPUT
		{
			public UInt32 Type;
			public KEYBOARDMOUSEHARDWARE Data;
		}
		[StructLayout(LayoutKind.Explicit)]
		//This is KEYBOARD-MOUSE-HARDWARE union INPUT won't work if you remove MOUSE or HARDWARE
        internal struct KEYBOARDMOUSEHARDWARE
		{
			[FieldOffset(0)]
			public KEYBDINPUT Keyboard;
			[FieldOffset(0)]
			public HARDWAREINPUT Hardware;
			[FieldOffset(0)]
			public MOUSEINPUT Mouse;
		}
		internal struct KEYBDINPUT
		{
			public UInt16 Vk;
			public UInt16 Scan;
			public UInt32 Flags;
			public UInt32 Time;
			public IntPtr ExtraInfo;
		}
		internal struct MOUSEINPUT
		{
			public Int32 X;
			public Int32 Y;
			public UInt32 MouseData;
			public UInt32 Flags;
			public UInt32 Time;
			public IntPtr ExtraInfo;
		}
		internal struct HARDWAREINPUT
		{
			public UInt32 Msg;
			public UInt16 ParamL;
			public UInt16 ParamH;
		}
		#pragma warning restore 649
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
		static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint extraInfo);
		const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
		const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
		const uint MOUSEEVENTF_LEFTUP = 0x0004;
		const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
		const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
		const uint MOUSEEVENTF_MOVE = 0x0001;
		const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
		const uint MOUSEEVENTF_RIGHTUP = 0x0010;
		const uint MOUSEEVENTF_XDOWN = 0x0080;
		const uint MOUSEEVENTF_XUP = 0x0100;
		const uint MOUSEEVENTF_WHEEL = 0x0800;
		const uint MOUSEEVENTF_HWHEEL = 0x01000;
		public enum MouseEventDataXButtons : uint
		{
			XBUTTON1 = 0x00000001,
			XBUTTON2 = 0x00000002
		}
		[DllImport("user32.dll")]
		static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData,
			int dwExtraInfo);
		[DllImport("user32.dll", SetLastError = true)]
		static extern int MapVirtualKey(uint uCode, uint uMapType);
		[DllImport("user32.dll", SetLastError = true)]
		static extern UInt32 SendInput(UInt32 numberOfInputs, INPUT[] inputs, Int32 sizeOfInputStructure);
		enum VK
		{
			ENTER = 0x0D,
			SHIFT = 0x10,
			CTRL = 0x11,
			MENU = 0x12,
			NUMPAD0 = 0x60,
			NUMPAD1 = 0x61,
			NUMPAD2 = 0x62,
			NUMPAD3 = 0x63,
			NUMPAD4 = 0x64,
			NUMPAD5 = 0x65,
			NUMPAD6 = 0x66,
			NUMPAD7 = 0x67,
			NUMPAD8 = 0x68,
			NUMPAD9 = 0x69,
			PageUp = 33,
			PageDown = 34,
			A = 65,
			B = 66,
			C = 67,
			D = 68,
			E = 69,
			F = 70,
			G = 71,
			H = 72,
			I = 73,
			J = 74,
			K = 75,
			L = 76,
			M = 77,
			N = 78,
			O = 79,
			P = 80,
			Q = 81,
			R = 82,
			S = 83,
			T = 84,
			U = 85,
			V = 86,
			W = 87,
			X = 88,
			Y = 89,
			Z = 90,
			OemOpenBrackets = 219,
			Backslash = 220,
			OemCloseBrackets = 221,
		}
		const uint KEYEVENTF_KEYUP = 0x0002;
		public const int INPUT_KEYBOARD = 1;
		
		[DllImport("user32.dll")]
		static extern bool SetCursorPos(int X, int Y);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetCursorPos(out POINT point);
		void 更新ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var s = Clipboard.GetText().Trim();
			if (s.Length > 0 && listBox1.SelectedIndex != -1) {
				var pieces = s.Split(new char[]{ '\n' }, 2);
			
				using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
					cmd.CommandText = @"update GeometryNode set Title = @NewTitle,Views = Views + 1,UpdateAt = (datetime('now','localtime')) where Title = @Title";
					cmd.Parameters.Add("Title", DbType.String).Value = listBox1.SelectedItem.ToString();
					cmd.Parameters.Add("NewTitle", DbType.String).Value = pieces[0].Trim();
					cmd.ExecuteNonQuery();
				}
				LoadData();
			}
		}
		void ListBox2DoubleClick(object sender, EventArgs e)
		{
			if (listBox2.SelectedIndex == -1) {
				//LoadData();
			
			} else {
				using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
					cmd.CommandText = @"select Name,Title from GeometryNode where Title = @Title";
					cmd.Parameters.Add("Title", DbType.String).Value = listBox2.SelectedItem.ToString();
					using (var reader = cmd.ExecuteReader()) {
						if (reader.Read())
							Clipboard.SetText(reader.GetString(1));
					}
				}
				using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
					cmd.CommandText = @"update GeometryNode set Views = Views + 1,UpdateAt = (datetime('now','localtime')) where Title = @Title";
					cmd.Parameters.Add("Title", DbType.String).Value = listBox2.SelectedItem.ToString();
					using (var reader = cmd.ExecuteReader()) {
						if (reader.Read())
							Clipboard.SetText(reader.GetString(0));
					}
				}
			}
		}
		void ListBox2SelectedIndexChanged(object sender, EventArgs e)
		{
	
		}
		void 新建ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var s = Clipboard.GetText().Trim();
			if (s.Length > 0) {
				var pieces = s.Split(new char[]{ '\n' }, 2);
			
				if (pieces.Length > 1) {
					//var key = pieces[0].Trim();
//				if (_snippets.ContainsKey(key))
//					_snippets[key] = pieces[1].Trim();
//				else
//					_snippets.Add(key, pieces[1].Trim());
//				File.WriteAllText(_fileName1, JsonConvert.SerializeObject(_snippets));
					using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
						cmd.CommandText = @"insert into GeometryNode(Title,Name) values(@Title,@Name)";
					
						cmd.Parameters.Add("Title", DbType.String).Value = pieces[0].Trim();
						cmd.Parameters.Add("Name", DbType.String).Value = pieces[1].Trim();
						cmd.ExecuteNonQuery();
					}
				 
				}
			}
		}

	}
}
