﻿using System;
using System.Web;
using System.Web.Mvc;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.AgentMetadata.Extensions;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class CreateBadgeInputModel : DefaultInputModel
	{
		public CreateBadgeInputModel()
		{
			CommandType = typeof (CreateBadge);
			AgentSystemName = CommandType.Assembly.GetAgentMetadata().SystemName;
		}

		public enum TriggerField
		{
			NumberPosts,
			NumberComments,
			CommentsOnPost,
			PostScore,
			CommentScore
		}

		public Guid ForumIdentifier { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public HttpPostedFileBase Image { get; set; }
		public string ImageUrl { get; set; }
		public string Field { get; set; }
		public string Operator { get; set; }
		public string Value { get; set; } 

		public SelectList Operators
		{
			get { return new SelectList(new [] {"<", "<=", "=", "<>", ">", ">="}); }
		}

		public SelectList Fields
		{
			get
			{ return new SelectList(Enum.GetNames(typeof(TriggerField)));}
		}
	}
}