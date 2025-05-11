 
using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Dir
{
	/// <summary>
	/// Description of Form2.
	/// </summary>
	public partial class Form2 : Form
	{
		SQLiteConnection conn;
		public Form2()
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
		}
		void ListBox1DoubleClick(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1) {
				//LoadData();
			
			} else {
				using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
					cmd.CommandText = @"select Name from GeometryNode where Title = @Title";
					cmd.Parameters.Add("Title", DbType.String).Value = listBox1.SelectedItem.ToString();
					using (var reader = cmd.ExecuteReader()) {
						if (reader.Read())
							Clipboard.SetText(File.ReadAllText("2.txt".GetEntryPath()).Replace("{0}",reader.GetString(0)));
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
					var regex = new Regex(comboBox1.Text.Trim());
					using (var reader = cmd.ExecuteReader()) {
						while (reader.Read()) {
							var x=reader.GetString(0);
							if (regex.IsMatch(reader.GetString(0)) || regex.IsMatch(reader.GetString(1))) {
								listBox1.Items.Add(reader.GetString(0));
							}
						}
					}
				}
			}
		}
		
	}
}
