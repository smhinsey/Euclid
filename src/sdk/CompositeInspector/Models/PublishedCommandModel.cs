﻿using Euclid.Framework.Cqrs;

namespace CompositeInspector.Models
{
    public class PublishedCommandModel : InspectorNavigationModel
    {
        public bool HasValue { get; set; }
        public ICommandPublicationRecord Record { get; set; }
        public string PublicationId { get; set; }
    }
}