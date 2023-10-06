using System;
using System.Collections.Generic;

namespace PetShopOnline.Models
{
    public partial class BlogDetail
    {
        public int BlogId { get; set; }
        public string? Content { get; set; }
        public string? Heading { get; set; }
        public string? PageTitle { get; set; }
        public string? ShortDescription { get; set; }
        public string? FeaturedImageUrl { get; set; }
        public DateTime? PublishedDate { get; set; }
    }
}
