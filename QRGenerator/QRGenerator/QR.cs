using QRCoder;

namespace QRGenerator
{
    public class QR
    {
        public static byte[] GenerateQr(string text)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q); //calidad del qr, Q es intermedia
            BitmapByteQRCode bitmapByteCode = new BitmapByteQRCode(qrCodeData);

            var bitMap = bitmapByteCode.GetGraphic(20);

            using var ms = new MemoryStream();
            ms.Write(bitMap);

            byte[] byteImage = ms.ToArray();

            return byteImage;
        }
    }
}
