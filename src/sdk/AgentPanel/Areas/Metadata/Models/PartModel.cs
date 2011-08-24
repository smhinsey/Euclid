﻿using Euclid.Framework.AgentMetadata;

namespace AgentPanel.Areas.Metadata.Models
{
	public class PartModel : FooterLinkModel
	{
		public string NextActionName { get; set; }
		public ITypeMetadata TypeMetadata { get; set; }
	}
}
