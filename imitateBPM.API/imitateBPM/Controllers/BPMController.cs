using imitateBPM.Entities.Database;
using imitateBPM.Extensions;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace imitateBPM.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BPMController : ControllerBase
    {
        private readonly ISqlSugarClient _db;
        public BPMController(ISqlSugarClient db)
        {
            _db = db;
        }
        [HttpGet(Name = "test")]
        public IActionResult Test()
        {
            var response = ResponseModelFactory.CreateInstance;
            var list = _db.Queryable<PdfTable>().First();
            response.SetSuccess();
            return Ok(response);
        }
        [HttpPost(Name ="UploadPDF")]
        public IActionResult UploadPDF(Models.Request.PDF data)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (data == null)
            {
                response.SetError("上传的数据为空");
                return Ok(response);
            }
            //var byteArray = System.Text.Encoding.UTF8.GetBytes(data.PdfData);
            var saveData = new PdfTable()
            {
                CreateTime = DateTime.Now,
                Data = data.PdfData,
                UseState = "f",
                fileName = data.fileName
            };
            try
            {
                _db.Insertable(saveData).ExecuteReturnEntity();
            }
            catch (Exception e)
            {
                response.SetError(e.Message);
                return Ok(response);
            }
            response.SetSuccess();
            return Ok(response);
        }
    }

}
