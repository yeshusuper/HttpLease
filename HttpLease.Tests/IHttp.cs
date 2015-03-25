using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpLease;

namespace HttpLease.Tests
{
    interface IHttp
    {
        [Url("/Api/Test")]
        string Test(long id, string value);
        [Url("/Api/Test/{id}")]
        string Test2(long id, string value);
        [Url("/Api/Test/{id}")]
        string Test3([Path(Name = "id")]long idPath, string value);
        [Url("/Api/Test")]
        string Test4(string value2);
        [FormUrlEncoded(IsEncodeValue = false)]
        [Url("/Api/Test")]
        string Test5(long id, string value);


        [HttpPost]
        [Url("/Api/Test")]
        string Post(long post, [Field(Name = "")]string value);
        [HttpPost]
        [Url("/Api/Test/{id}")]
        string Post2(long id, string value);
        [HttpPost]
        [Url("/Api/Test/{id}")]
        string Post3(long id, string value, [Field(Name = "")]string value2);
        [HttpPost]
        [Url("/Api/Obj/{id}")]
        string Post4(long id, [FieldMap]PostRequest value, [FieldMap]PostRequest2 value2);
        [Multipart]
        [HttpPost]
        [Url("/File/Index")]
        string PostFile([Part]string file);


        [HttpDelete]
        [Url("/Api/Test")]
        string Delete(long delete, [Field(Name = "")]string value);
        [HttpDelete]
        [Url("/Api/Test/{id}")]
        string Delete2(long id, string value);
        [HttpDelete]
        [Url("/Api/Test/{id}")]
        string Delete3(long id, string value, [Field(Name = "")]string value2);


        [HttpPut]
        [Url("/Api/Test")]
        string Put(long put, [Field(Name = "")]string value);
        [HttpPut]
        [Url("/Api/Test/{id}")]
        string Put2(long id, string value);
        [HttpPut]
        [Url("/Api/Test/{id}")]
        string Put3(long id, string value, [Field(Name = "")]string value2);
    }
}
