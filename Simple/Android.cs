using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
public static class Android
{
	static void EncodeBin()
	{
		ClipboardShare.SetText(
			File.ReadAllText("1.txt".GetEntryPath()).Replace("``", "`" + ClipboardShare.GetText() + "`")
		);
	}

	private static void FormatBlenderScript(TextBox textBox, Func<float[],string> fun, string defaultValue = "-.")
	{
		int i = textBox.SelectionStart;
		int jv = i;
		while (i > 0 && textBox.Text[i - 1] != '\n') {
			i--;
		}
		while (jv + 1 < textBox.Text.Length && textBox.Text[jv] != '\n') {
			jv++;
		}
		var value = textBox.Text.Substring(i, jv - i).Trim();
		try {
			var s = "";
			var n = Regex.Matches(value, "[\\d.-]+").Cast<Match>().Select(v => float.Parse(v.Value)).ToArray();
			if (Regex.IsMatch(value, "x", RegexOptions.IgnoreCase)) {
				s = fun(new float[]{ n[0], 0, 0 });
			} else if (Regex.IsMatch(value, "y", RegexOptions.IgnoreCase)) {
				s = fun(new float[]{ 0, n[0], 0 });
			} else if (Regex.IsMatch(value, "z", RegexOptions.IgnoreCase)) {
				s =	fun(new float[]{ 0, 0, n[0] });
			} else {
				s = fun(new float[]{ n[0], n[0], n[0] });
			} 
			ClipboardShare.SetText(s);
			textBox.Text = textBox.Text.Substring(0, i) + s + Environment.NewLine + textBox.Text.Substring(i + value.Length);
			textBox.SelectionStart = i;
		} catch (Exception e) {
			if (textBox.SelectionStart == 0) {
				textBox.SelectedText = Environment.NewLine + Environment.NewLine + defaultValue; // e.Message + e.StackTrace + Environment.NewLine + textBox.Text;
				//textBox.SelectionStart += defaultValue.Length;
	
			} else {
				textBox.SelectedText = defaultValue; // e.Message + e.StackTrace + Environment.NewLine + textBox.Text;
				//textBox.SelectionStart += defaultValue.Length;
	
			}
		}
	}
	public static void HandleKeyDown(TextBox textBox, KeyEventArgs arg)
	{
		
		if (arg.KeyCode == Keys.F1) {
			FormatBlenderScript(textBox, v =>
				                string.Format("bpy.ops.transform.resize(value=({0}, {1}, {2}))", v[0] == 0 ? 1 : v[0], v[1] == 0 ? 1 : v[1], v[2] == 0 ? 1 : v[2]));
			// \r\nbpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
		} else if (arg.KeyCode == Keys.F2) {
			FormatBlenderScript(textBox, v =>
				                    string.Format("bpy.ops.mesh.extrude_region_move()\r\nbpy.ops.transform.translate(value=({0}, {1}, {2}))", v[0], v[1], v[2]), ".");
		} else if (arg.KeyCode == Keys.F3) {
			FormatBlenderScript(textBox, v => string.Format("bpy.ops.transform.translate(value=({0}, {1}, {2}))", v[0], v[1], v[2]), "1.");
		} else if (arg.KeyCode == Keys.F4) {
			FormatBlenderScript(textBox, v => {
				return string.Format("bpy.ops.mesh.inset(thickness={0}, depth=0)", v[0], v[1], v[2]);
			});
		} else if (arg.KeyCode == Keys.F5) {
			FormatBlenderScript(textBox, v => {
				return string.Format("bpy.ops.mesh.bevel(offset={0},segments=1, affect='EDGES')", v[0], v[1], v[2]);
			});
			
			
		} else if (arg.KeyCode == Keys.F6) {
			FormatBlenderScript(textBox, v => {
				return string.Format(@"bpy.ops.mesh.extrude_region_shrink_fatten(TRANSFORM_OT_shrink_fatten={{""value"":{0},""use_even_offset"":True}})
", v[0]);
			});
		} else if (arg.KeyCode == Keys.F7) {
			/*var str=File.ReadAllText("2.txt".GetEntryPath());
			var ss=ClipboardShare.GetText().Trim().Split(new char[]{' '},StringSplitOptions.RemoveEmptyEntries);
			var s1="_"+string.Join("_",ss);
			var s2=string.Join(".",ss);
			var s3=string.Join("_",ss);
			str=Regex.Replace(str,"\\{1}",s1);
			str=Regex.Replace(str,"\\{2}",s2);
			str=Regex.Replace(str,"\\{3}",s3);
			
			ClipboardShare.SetText(str);*/
			var array = new []{ "X", "Y", "Z" };
			var s1 = array.Select((x, i) => string.Format(@"class _quick_rotate_{0}(Operator):
    """""" Selection group """"""
    bl_idname = ""quick.rotate{0}""
    bl_label = """"
    bl_options = {{""REGISTER"", ""UNDO""}}

    @classmethod
    def poll(cls, context):
        return context.mode == ""OBJECT"" or context.mode == ""EDIT_MESH""

    def execute(self, context):
        quick_rotate({1})
        return {{'FINISHED'}}", x.ToLower(), i));
			
			var s2 = array.Select((x, i) => string.Format(@"   elif mode=={0}:", i));
			var s3 = array.Select((x, i) => string.Format(@"        row.operator(_quick_rotate_{0}.bl_idname, text=""{1}"")", x.ToLower(), x));
			var s4 = string.Join(Environment.NewLine, array.Select((x, i) => string.Format(@"    _quick_rotate_{0},", x.ToLower())));
			ClipboardShare.SetText("def quick_rotate(mode):\r\n" + string.Join(Environment.NewLine, s2) + "    return None\r\n\r\n" + string.Join(Environment.NewLine, s1) + "\r\n" + string.Join(Environment.NewLine, s3) + "\r\n" + s4);
		} else if (arg.KeyCode == Keys.F11) {
			ClipboardShare.SetText(ClipboardShare.GetText().FormatString());
		} else if (arg.KeyCode == Keys.Oem3) {
			
			var s = ClipboardShare.GetText().Trim().Split(new char[]{ ' ' }, StringSplitOptions.RemoveEmptyEntries)
				.Select((x, i) => {
				return string.Format(@"        elif mode=={0}:
            t=""{1}""", i + 1, x);
			});
			
			ClipboardShare.SetText(string.Join(Environment.NewLine, s));
			/*
			 string.Format(@"class _view_axis_{0}(Operator):
    """""" Selection group """"""
    bl_idname = ""view.axis{0}""
    bl_label = """"
    bl_options = {{""REGISTER"", ""UNDO""}}

    @classmethod
    def poll(cls, context):
        return context.mode == ""OBJECT""

    def execute(self, context):
        view_axis({1})
        return {{'FINISHED'}}",x.ToLower(),i+1);
			 */
			
		} else if (arg.KeyCode == Keys.Q) {
			
			//ClipboardShare.SetText("bpy.ops.object.modifier_add(type='MIRROR')");
		} else if (arg.KeyCode == Keys.W) {
//			ClipboardShare.SetText(@"bpy.ops.object.modifier_add(type='BEVEL')
//bpy.context.object.modifiers[""Bevel""].angle_limit = 0.785398
//bpy.context.object.modifiers[""Bevel""].segments = 2
//bpy.context.object.modifiers[""Bevel""].harden_normals = True
//bpy.context.object.modifiers[""Bevel""].width = 0.013
//bpy.ops.object.subdivision_set(level=2, relative=False)
//bpy.ops.object.shade_smooth()
//");
		
		} else if (arg.KeyCode == Keys.S) {
			//ClipboardShare.SetText("bpy.ops.object.modifier_add(type='SOLIDIFY')");
		} 
//		else if (arg.KeyCode == Keys.L) {
//			FormatBlenderScript(textBox, v => {
//				var o1 = 1 - v[0];
//				var o2 = (1 - o1 / 2) / 2;
//				var o3 = o2 - o1 / 2;
//				return (o3 / o2 * -1).ToString();
//			});
//		}
		if (arg.Control) {
			if (arg.KeyCode == Keys.C) {
				if (textBox.SelectedText.Length > 0) {
					return;
				}
				var start = textBox.SelectionStart;
				var end = start;
				while (start - 1 > -1) {
					var founded = false;
					if (textBox.Text[start] == '\n') {
						var p = start - 1;
						while (p - 1 > -1) {
							if (textBox.Text[p] == '\n') {
								if (string.IsNullOrWhiteSpace(textBox.Text.Substring(p, start - p))) {
									founded = true;
									start++;
									break;
								}
							}
							p--;
						}
					}
					if (founded)
						break;
					start--;
				}
				while (end + 1 < textBox.Text.Length) {
					var founded = false;
					if (textBox.Text[end] == '\n') {
						var p = end + 1;
						while (p + 1 < textBox.Text.Length) {
							if (textBox.Text[p] == '\n') {
								if (string.IsNullOrWhiteSpace(textBox.Text.Substring(end, p - end))) {
									founded = true;
									end++;
									break;
								}
							}
							p++;
						}
					}
					if (founded)
						break;
					end++;
				}
				ClipboardShare.SetText(textBox.Text.Substring(start, end - start + 1).Trim());
			} else if (arg.KeyCode == Keys.F) {
				ReplaceString(textBox);
				/*var s = Clipboard.GetText().Trim();
				var s1 = string.Format(@"class _quick_{0}(Operator):
    """""" Quick {0} """"""
    bl_idname = ""quick.{0}""
    bl_label = """"
    bl_options = {{""REGISTER"", ""UNDO""}}

    @classmethod
    def poll(cls, context):
        return context.mode == ""EDIT_MESH"" or context.mode == ""OBJECT""

    def execute(self, context):
        value = int(bpy.context.window_manager.clipboard)
        return {{'FINISHED'}}
		
        row.operator(_quick_{0}.bl_idname, text=""{1}"")
		
    _quick_{0},", s, s.Capitalize());
				Clipboard.SetText(s1);
			}  else if (arg.KeyCode == Keys.L) {
				FormatBlenderScript(textBox, v => {
					var o1 = 1 - v[0];
					var o2 = (1 - o1 / 2) / 2;
					var o3 = o2 - o1 / 2;
					return (o3 / o2 * -1).ToString();
				});
			}*/
			} else if (arg.KeyCode == Keys.G) {
			
				textBox.SelectedText = Utils.Translate(ClipboardShare.GetText());
			}
			return;
			if (arg.Alt) {
				if (arg.KeyCode == Keys.Q) {
					ClipboardShare.SetText("bpy.ops.object.align(bb_quality=True, align_mode='OPT_1', relative_to='OPT_2', align_axis={ 'X'})");
				} else if (arg.KeyCode == Keys.W) {
					ClipboardShare.SetText("bpy.ops.object.align(bb_quality=True, align_mode='OPT_2', relative_to='OPT_2', align_axis={ 'X'})");
				} else if (arg.KeyCode == Keys.E) {
					ClipboardShare.SetText("bpy.ops.object.align(bb_quality=True, align_mode='OPT_3', relative_to='OPT_2', align_axis={ 'X'})");
				} else if (arg.KeyCode == Keys.A) {
					ClipboardShare.SetText("bpy.ops.object.align(bb_quality=True, align_mode='OPT_1', relative_to='OPT_2', align_axis={ 'Y'})");
				} else if (arg.KeyCode == Keys.S) {
					ClipboardShare.SetText("bpy.ops.object.align(bb_quality=True, align_mode='OPT_2', relative_to='OPT_2', align_axis={ 'Y'})");
				} else if (arg.KeyCode == Keys.D) {
					ClipboardShare.SetText("bpy.ops.object.align(bb_quality=True, align_mode='OPT_3', relative_to='OPT_2', align_axis={ 'Y'})");
				} else if (arg.KeyCode == Keys.Z) {
					ClipboardShare.SetText("bpy.ops.object.align(bb_quality=True, align_mode='OPT_1', relative_to='OPT_2', align_axis={ 'Z'})");
				} else if (arg.KeyCode == Keys.X) {
					ClipboardShare.SetText("bpy.ops.object.align(bb_quality=True, align_mode='OPT_2', relative_to='OPT_2', align_axis={ 'Z'})");
				} else if (arg.KeyCode == Keys.C) {
					ClipboardShare.SetText("bpy.ops.object.align(bb_quality=True, align_mode='OPT_3', relative_to='OPT_2', align_axis={ 'Z'})");
				}
				return;
			} else if (arg.Shift) {
				if (arg.KeyCode == Keys.P) {
					ClipboardShare.SetText("bpy.ops.mesh.primitive_plane_add(enter_editmode=True)");
				} else if (arg.KeyCode == Keys.U) {
					ClipboardShare.SetText("bpy.ops.mesh.primitive_cube_add(enter_editmode=True)");
				} else if (arg.KeyCode == Keys.C) {
					ClipboardShare.SetText("bpy.ops.mesh.primitive_circle_add(enter_editmode=True)");
				} else if (arg.KeyCode == Keys.Y) {
					// vertices=12,
					ClipboardShare.SetText("bpy.ops.mesh.primitive_cylinder_add( enter_editmode=True)");
				} 
				return;
			}
//		if (arg.KeyCode == Keys.F1) {
//			EncodeBin();
//		}
//		if (arg.KeyCode == Keys.F2) {
//			TransalteVar();
//		}
//		if (arg.KeyCode == Keys.F3) {
//			CreateFile(textBox);
//		}
//		 
//		if (arg.KeyCode == Keys.F4) {
//			//FormatLog();
//			/*var s=ClipboardShare.GetText();
//			var array=new string[]{
//				s.SubstringBefore("{{shader}}"),
//				s.SubstringAfter("{{shader}}"),
//			};
//			ClipboardShare.SetText("const THREE1 = "+Newtonsoft.Json.JsonConvert.SerializeObject(array));
//			
//				
//				 ClipboardShare.SetText(
//								File.ReadAllText("1.txt".GetEntryPath()).Replace("{{shader}}",ClipboardShare.GetText())
//							);
//				 */
//			ColorPicker();
//		} else if (arg.KeyCode == Keys.F7) {
//			ReplaceString(textBox);
//		} 
		
		 
		}
	}
	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool GetCursorPos(out POINT point);
	[StructLayout(LayoutKind.Sequential)]
	struct POINT
	{
		public Int32 X;
		public Int32 Y;
	}
	public static void ColorPicker()
	{
		POINT p = new POINT();
		GetCursorPos(out p);
		int mouseX = p.X;
		int mouseY = p.Y;
		var screen = Screen.FromRectangle(new Rectangle(p.X, p.Y, 1, 1));
		var	sampleBitmap = GetSampleRegion(screen, mouseX, mouseY);
		Color sampleColor = sampleBitmap.GetPixel(sampleSize / 2, sampleSize / 2);
		sampleBitmap.Dispose();
//			string tmpR = (sampleColor.R / 255f).ToString("0.##f", CultureInfo.GetCultureInfo("en-us"));
//			string tmpG = (sampleColor.G / 255f).ToString("0.##f", CultureInfo.GetCultureInfo("en-us"));
//			string tmpB = (sampleColor.B / 255f).ToString("0.##f", CultureInfo.GetCultureInfo("en-us"));
//			var s = string.Format("{0}, {1}, {2}", tmpR, tmpG, tmpB);
		var s = ColorTranslator.ToHtml(sampleColor);
		Clipboard.SetText(s.Substring(1));
	}
	public static void ColorPicker(TextBox textBox)
	{
		POINT p = new POINT();
		GetCursorPos(out p);
		int mouseX = p.X;
		int mouseY = p.Y;
		var screen = Screen.FromRectangle(new Rectangle(p.X, p.Y, 1, 1));
		var	sampleBitmap = GetSampleRegion(screen, mouseX, mouseY);
		Color sampleColor = sampleBitmap.GetPixel(sampleSize / 2, sampleSize / 2);
		sampleBitmap.Dispose();
		var s = ColorTranslator.ToHtml(sampleColor);
		textBox.SelectedText = string.Format("{0}\r\n{1}\r\n{2}\r\n{3}", sampleColor.R, sampleColor.G, sampleColor.B, s);
	}
	
	static	int sampleSize = 5;
	private static Bitmap GetSampleRegion(Screen screen, int mouseX, int mouseY)
	{
		var bmp = new Bitmap(sampleSize, sampleSize, PixelFormat.Format32bppArgb);
		Graphics gfxScreenshot = Graphics.FromImage(bmp);
		gfxScreenshot.CopyFromScreen(mouseX - sampleSize / 2, mouseY - sampleSize / 2, 0, 0, new Size(sampleSize, sampleSize));
		gfxScreenshot.Save();
		gfxScreenshot.Dispose();
		return bmp;
	}
	private static void FormatLog()
	{
		//  *[a-zA-Z0-9_()]+(?=,| = )
		// (?<= )[a-zA-Z0-9_]+(?= = )
		// Regex.Matches(ClipboardShare.GetText(), "([a-zA-Z0-9_()]+(?=,))|([a-zA-Z0-9_()]+\\b)").Cast<Match>().Select(x => x.Value.Trim());			//ClipboardShare.GetText().Split(',').Select(x=>x.Trim());
		
		var s = ClipboardShare.GetText().Split(',').Select(x => x.Trim());
		var v = string.Join("", s.Select(x => string.Format("{0} = %s;\\n", x)));
		ClipboardShare.SetText(string.Format("Log.e(\"B5aOx2\", String.format(\"[]: {0}\",{1}));", v, string.Join(",", s)));
	}
	private static void TransalteVar()
	{
		var s = Utils.Translate();
		ClipboardShare.SetText(s.Camel().Decapitalize());
	}
	
	private static void FormatString()
	{
		var suffix = new string[] {
			"Width", "Height", "Top", "Left", "Right", "Bottom", "X", "Y", "Start", "End"
		};
		var s = Clipboard.GetText().Trim();
		
		if (s.Contains("{0}")) {
			Clipboard.SetText(string.Join("\r\n", suffix.Select(
				x => string.Format(s, x)
			)));
		} else {
			Clipboard.SetText(
				string.Format("{0}\r\n\r\n{1}",
					string.Join("\r\n", suffix.Select(
						x => string.Format("int {0}{1} = 0;", s, x)
					)),
					string.Join("\r\n", suffix.Select(
						x => string.Format("private int {0}{1};", s, x)
					))
				));
		}
		
		
	}
	
	private static void ReplaceString(TextBox textBox)
	{
		var s = textBox.Text.Trim();
		var first = s.SubstringBefore('\n').Trim();
		var second = s.SubstringAfter('\n').Trim();
		if (Regex.IsMatch(first, "^\\d+$")) {
			var sb = new StringBuilder();
			for (int i = 0; i < int.Parse(first); i++) {
				sb.AppendLine(Regex.Replace(second, "\\$\\d+", (i + 1 + int.Parse(Regex.Match(second, "(?<=\\$)\\d+").Value)).ToString()));
			}
			textBox.Text = first + "\r\n" + sb.ToString();
		} else if (Regex.IsMatch(first, "^\\$\\d+$")) {
			var sb = new StringBuilder();
			for (int i = 0; i < int.Parse(first); i++) {
				sb.AppendLine(Regex.Replace(second, "\\$\\d+", (i + 1).ToString()));
			}
			textBox.Text = first + "\r\n" + sb.ToString();
		} else if (first.StartsWith(".")) {
			first.TrimStart('.').CreateFileIfNotExists();
		} else if (first.StartsWith("_")) {
		
			var path = first.TrimStart('_');
			var fileName = Path.GetFileName(path);
			System.IO.Compression.ZipFile.ExtractToDirectory("Douyin5Guang.zip".GetEntryPath(),
				Path.GetDirectoryName(path));
			var dir = Directory.GetDirectories(Path.GetDirectoryName(path)).First();
			var src = Path.GetFileName(dir);
			Directory.Move(dir, path);
			var files = Directory.GetFiles(Path.Combine(path,"app\\src"), "*.java", SearchOption.AllDirectories);
			foreach (var element in files) {
				File.WriteAllText(element, File.ReadAllText(element).Replace(
					src.ToLower(), fileName.ToLower()
				));
			}
			var srcParent = Path.GetDirectoryName(files.First());
			Directory.Move(srcParent, Path.Combine(Path.GetDirectoryName(srcParent), fileName.ToLower()));
			               
			files = Directory.GetFiles(path, "*.kts", SearchOption.AllDirectories);
			foreach (var element in files) {
				File.WriteAllText(element, File.ReadAllText(element).Replace(
					src.ToLower(), fileName.ToLower()
				));
			}
			
		} else {
			var array = first.Split(' ');
			if (array.Length > 1)
				textBox.Text = first + "\r\n" + second.Replace(
					array[0], array[1]
			
				);
		}
		
	}
	private static void CreateFile(TextBox textBox)
	{
		var s = textBox.Text.Trim();
		var first = s.SubstringBefore('\n').Trim();
		
		if (File.Exists(first)) {
			return;
		}
		Path.GetDirectoryName(first).CreateDirectoryIfNotExists();
		File.WriteAllText(first, string.Empty);
	}
}