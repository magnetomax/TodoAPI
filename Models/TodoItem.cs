using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long TodoItemID { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsComplete { get; set; }

        public TodoItem()
       {          
         this.CreatedDate  = DateTime.UtcNow;
         this.ModifiedDate = DateTime.UtcNow;
       }
    }
}