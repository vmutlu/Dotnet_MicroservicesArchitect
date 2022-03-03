using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAuctionApp.WEBUI.Models
{
    public class AuctionModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please fill Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please fill Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please fill ProductId")]
        public string ProductId { get; set; }

        [Required(ErrorMessage = "Please fill Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please fill StartedDate")]
        public DateTime StartedDate { get; set; }

        [Required(ErrorMessage = "Please fill FinishedDate")]
        public DateTime FinishedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public int SellerId { get; set; }
        public List<string> Sellers { get; set; }
    }
}
