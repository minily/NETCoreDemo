using System;

namespace BaseDemo001.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);


        public string validId => RequestId ?? "id";

        public string testStr => "id";
    }
}
