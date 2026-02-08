using System;
using System.Collections.Generic;

namespace ChatServer.Api.Models
{
    public class Dialog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public List<int> MemberIds { get; set; } = new List<int>();
    }
}
