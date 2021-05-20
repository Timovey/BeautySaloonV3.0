using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace BeautySaloonBusinessLogic.HelperModels
{
    class PdfCellParameters
    {
        public Cell Cell { get; set; }

        public string Text { get; set; }

        public string Style { get; set; }

        public ParagraphAlignment ParagraphAlignment { get; set; }

        public Unit BorderWidth { get; set; }
    }
}