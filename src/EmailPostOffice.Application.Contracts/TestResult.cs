using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EmailPostOffice
{

    [Serializable]
    [JsonSerializable(typeof(TestResultDto))]
    public class TestResultDto
    {
        public string Result { get; set; }
    }
}
