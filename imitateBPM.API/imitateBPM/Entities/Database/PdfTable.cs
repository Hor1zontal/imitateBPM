using SqlSugar;

namespace imitateBPM.Entities.Database
{
    public class PdfTable
    {
        public PdfTable() { }

        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int? ID { get; set; }

        [SugarColumn(ColumnDataType = "Nvarchar(255)")]
        public string fileName { get; set; }
        //ColumnDataType 自定格式的情况 length不要设置 （想要多库兼容看4.2和9）
        [SugarColumn(ColumnDataType = "Nvarchar(255)")]
        public string UseState { get; set; }

        public string Data { get; set; }
        public DateTime? CreateTime { get; set; }

    }
}
