using iText.Bouncycastleconnector;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;
using VitaFlow.Presenation.Extensions;

namespace VitaFlow.Presenation.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly I_Transaction i_Transaction;

        public InvoiceController(I_Transaction i_Transaction)
        {
            this.i_Transaction = i_Transaction;
        }

        public async Task<IActionResult> DownloadPdf(string BillNo)
        {
            DistributorBillModel model = new DistributorBillModel();

            var base64DecodedBytes = Convert.FromBase64String(BillNo);
            string BillNoValue = Encoding.UTF8.GetString(base64DecodedBytes);

            string CurrentPartyCode = "";

            var billsoldby = await i_Transaction.GetSoldBy(BillNoValue, "F");

            if (billsoldby != null && billsoldby.SoldBy != "")
            {
                CurrentPartyCode = billsoldby.SoldBy;
                BillNoValue = billsoldby.BillNo;
            }

            model = await i_Transaction.getInvoice(BillNoValue, CurrentPartyCode, "F");

            string result = await this.RenderRazorViewToStringAsync("../Transaction/InvoicePrintPDF", model);

            var pdfBytes = HtmlToPdf(result);

            return File(pdfBytes, "application/pdf", "Invoice_" + model.BillNo + ".pdf");
        }

        public byte[] HtmlToPdf(string html)
        {
            using var stream = new MemoryStream();

            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);

            pdf.SetDefaultPageSize(PageSize.A4);

            var properties = new ConverterProperties();

            HtmlConverter.ConvertToPdf(html, pdf, properties);

            pdf.Close();

            return stream.ToArray();
        }

    }
}
