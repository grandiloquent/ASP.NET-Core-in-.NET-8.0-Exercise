using System.Drawing;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Ocr.V20181119;
using TencentCloud.Ocr.V20181119.Models;

public class ImageUtils
{
    public static byte[] ScreenShoot(Point p1, Point p2, int threshold = 128)
    {
        //			int screenLeft = SystemInformation.VirtualScreen.Left;
        //			int screenTop = SystemInformation.VirtualScreen.Top;
        //			int screenWidth = SystemInformation.VirtualScreen.Width;
        //			int screenHeight = SystemInformation.VirtualScreen.Height;


        // Create a bitmap of the appropriate size to receive the full-screen screenshot.
        using (Bitmap bitmap = new Bitmap(p2.X - p1.X, p2.Y - p1.Y))
        {
            // Draw the screenshot into our bitmap.
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(p1.X, p1.Y, 0, 0, bitmap.Size);
            }
            var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            bitmap.Dispose();
            return ms.ToArray();
        }
    }
    public static string Ocr(Point p1, Point p2)
    {
        var buf = ScreenShoot(p1, p2);
        if (buf == null)
            return null;
        const string AppId = KeyShare.AppId;
        const string
        SecretKey = KeyShare.SecretKey;
        const string
        SecretId = KeyShare.SecretId;
        const string Bucket = "tencentyun";
        const string Host = "recognition.image.myqcloud.com";
        try
        {
            // 实例化一个认证对象，入参需要传入腾讯云账户 SecretId 和 SecretKey，此处还需注意密钥对的保密
            // 代码泄露可能会导致 SecretId 和 SecretKey 泄露，并威胁账号下所有资源的安全性。以下代码示例仅供参考，建议采用更安全的方式来使用密钥，请参见：https://cloud.tencent.com/document/product/1278/85305
            // 密钥可前往官网控制台 https://console.cloud.tencent.com/cam/capi 进行获取
            Credential cred = new Credential
            {
                SecretId = SecretId,
                SecretKey = SecretKey
            };
            // 实例化一个client选项，可选的，没有特殊需求可以跳过
            ClientProfile clientProfile = new ClientProfile();
            // 实例化一个http选项，可选的，没有特殊需求可以跳过
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.Endpoint = ("ocr.tencentcloudapi.com");
            clientProfile.HttpProfile = httpProfile;
            // 实例化要请求产品的client对象,clientProfile是可选的
            OcrClient client = new OcrClient(cred, "ap-guangzhou", clientProfile);
            // 实例化一个请求对象,每个接口都会对应一个request对象
            GeneralBasicOCRRequest req = new GeneralBasicOCRRequest()
            {
                /*
        	 File.ReadAllBytes(ClipboardShare.GetFileNames()
        	                                                       .First(File.Exists))
					 */
                ImageBase64 = Convert.ToBase64String(buf.ToArray())
            };
            // 返回的resp是一个EnglishOCRResponse的实例，与请求对象对应
            GeneralBasicOCRResponse resp = client.GeneralBasicOCRSync(req);
            var s = String.Join(Environment.NewLine + Environment.NewLine, resp.TextDetections.Select(x => x.DetectedText));
            return s;
        }
        catch (Exception e)
        {
            return null;
        }
    }

}