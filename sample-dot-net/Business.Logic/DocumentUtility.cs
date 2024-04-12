using System;
using Aspose.Pdf;
using Aspose.Pdf.Facades;

namespace sample_dot_net.Business.Logic
{
    public static class DocumentUtility
    {
        public static byte[] PdfPutWatermark(byte[] inputBytes, string userfirstname, string userlastname, decimal? fontsize)
        {
            MemoryStream inputStream = new MemoryStream(inputBytes);
            MemoryStream outputStream = new MemoryStream();
            License license = new License();
            license.SetLicense(@"Aspose.Total.NET.lic");
            try
            {
                PdfFileInfo fi = new PdfFileInfo(inputStream);
                if (fi.IsPdfFile)
                {
                    string userName = userfirstname + " " + userlastname;
                    string strWatermark = "CONFIDENTIAL " + (userName.Count() > 20 ? userName.Substring(0, 20) : userName);
                    inputStream.Position = 0;

                    PdfFileStamp stamper = new PdfFileStamp();
                    stamper.BindPdf(inputStream);
                    Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(inputStream);

                    FormattedText tx = new FormattedText(strWatermark,
                        new FontColor(211, 211, 211),
                        FontStyle.Courier,
                        EncodingType.Winansi,
                        true,
                        (float)((16 <= fontsize && fontsize <= 42) ? fontsize : 30));
                    tx.AddNewLineText(String.Format("{0:yyyy-MM-dd H:mm:ss}", DateTime.UtcNow));
                    foreach (Aspose.Pdf.Page pdfPage in pdfDocument.Pages)
                    {
                        TextStamp stamp = new TextStamp(tx);
                        stamp.HorizontalAlignment = HorizontalAlignment.Center;
                        stamp.VerticalAlignment = VerticalAlignment.Center;
                        stamp.TextAlignment = HorizontalAlignment.Center;
                        stamp.RotateAngle = Math.Round(Math.Atan2(stamper.PageHeight, stamper.PageWidth) * 180 / Math.PI) * -1;
                        stamp.Opacity = 0.5;
                        stamp.Background = false;
                        pdfPage.AddStamp(stamp);
                    }
                    pdfDocument.Save(outputStream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during the watermarking process and the zip file could not be created.  The watermarking option [IsWatermark] can be found in the config file.", ex);
            }

            return outputStream.ToArray();
        }
    }
}

