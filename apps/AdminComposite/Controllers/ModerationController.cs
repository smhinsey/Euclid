using System;
using System.Globalization;
using System.Web.Mvc;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class ModerationController : Controller
	{
		private readonly ModeratedPostQueries _postQueries;
		private readonly ModeratedCommentQueries _commentQueries;
		private readonly IPublisher _publisher;

		public ModerationController(ModeratedPostQueries postQueries, ModeratedCommentQueries commentQueries, IPublisher publisher)
		{
			_postQueries = postQueries;
			_publisher = publisher;
			_commentQueries = commentQueries;
		}

		public ActionResult Items(Guid forumId, int offset = 0, int pageSize = 5, string type = "posts")
		{
			ViewBag.Title = "Moderate Forum " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(type);
			ViewBag.ItemType = type;

			ModeratedItems model = null;
			if (type.ToLower() == "posts")
			{
				model = _postQueries.ListUnapprovedPosts(forumId, offset, pageSize);
			}
			else if (type.ToLower() == "comments")
			{
				model = _commentQueries.ListUnapprovedComments(forumId, offset, pageSize);
			}
			else
			{
				throw new ArgumentOutOfRangeException("type");
			}

			ViewBag.Pagination = GetPagination("Posts", model.Offset, model.TotalPosts, model.PageSize);

			return View("Items", model);
		}

		public JsonResult ApprovePost(Guid postId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);
			var publicationId = _publisher.PublishMessage(new ApprovePost
															{
																ApprovedBy = userId,
																CreatedBy = userId,
																PostIdentifier = postId
															});

			return Json(publicationId, JsonRequestBehavior.AllowGet);
		}

		public JsonResult RejectPost(Guid postId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);
			var publicationId = _publisher.PublishMessage(new RejectPost
															{
																CreatedBy = userId,
																PostIdentifier = userId
															});

			return Json(publicationId, JsonRequestBehavior.AllowGet);
		}

		public JsonResult ApproveComment(Guid postId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);
			var publicationId = _publisher.PublishMessage(new ApproveComment
			                                              	{
			                                              		ApprovedBy = userId,
			                                              		CreatedBy = userId,
			                                              		CommentIdentifier = postId
			                                              	});

			return Json(publicationId, JsonRequestBehavior.AllowGet);
		}

		public JsonResult RejectComment(Guid postId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);
			var publicationId = _publisher.PublishMessage(new RejectComment
			{
				CreatedBy = userId,
				CommentIdentifier = postId
			});

			return Json(publicationId, JsonRequestBehavior.AllowGet);
		}

		private PaginationModel GetPagination(string actionName, int offset, int totalPosts, int pageSize)
		{
			return new PaginationModel
										{
											ActionName = actionName,
											ControllerName = "Moderation",
											Offset =offset,
											TotalPosts = totalPosts,
											PageSize = pageSize,
											WriteTFoot = true,
											WriteTable = true
										};
		}

//        public ActionResult GeneratePosts(Guid forumId, int numberPosts)
//        {
//            var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

//            var rand = new Random();
//            for (int i = 0; i < numberPosts; i++)
//            {
//                var body = TestPosts[rand.Next(7)];

//                if (i == 3 || rand.Next(100) % 24 == 0)
//                {
//                    body = string.Format("{0}<br/>{1}<br/>boo-ya", body, body);
//                } 
//                else if (rand.Next(100) % 5 == 0)
//                {
//                    body = body.Substring(0, body.Length/(rand.Next(2, 25)));
//                } else if (rand.Next(100) % 17 == 0)
//                {
//                    body = "a really short post";
//                }

//                _publisher.PublishMessage(new PublishPost
//                                            {
//                                                AuthorIdentifier = userId,
//                                                CreatedBy = userId,
//                                                Body = body,
//                                                Title = string.Format("A test post {0}", i),
//                                                ForumIdentifier = forumId,
//                                                ModerationRequired = true,
//                                                Identifier = Guid.NewGuid(),
//                                                Created = DateTime.Now
//                                            });
//            }
				
//            return RedirectToAction("Posts", new { forumId });
//        }

//        private static readonly List<string> TestPosts = new List<string>
//                                                    {
//@"Lebowski ipsum ya see? Nothing is fucked up here, Dude. Nothing is fucked. These guys are fucking amateurs. Dolor sit amet, consectetur adipiscing elit praesent. Ac magna justo pellentesque ac lectus quis. Brandt can't watch though. Or he has to pay a hundred. Sex. The physical act of love. Coitus. Do you like it? Elit blandit fringilla a ut turpis praesent. Felis ligula, malesuada suscipit malesuada non, ultrices. Fucking fascist!",
//@"Mark that frame an eight, you're entering a world of pain. Non urna sed orci ipsum, placerat id condimentum rutrum, rhoncus. Ac lorem aliquam placerat posuere neque, at dignissim magna ullamcorper. I told that kraut a fucking thousand times I don't roll on shabbos. That's fuckin' combat. The man in the black pyjamas, Dude. Worthy fuckin' adversary. In aliquam sagittis massa ac tortor ultrices faucibus curabitur eu.",
//@"I'm not Mr. Lebowski; you're Mr. Lebowski. I'm the Dude. Mi sapien, ut ultricies ipsum. Morbi eget risus nulla nullam. Let me tell you something, pendejo. You pull any your crazy shit with us, you flash a piece out on the lanes, I'll take it away from you and stick it up your ass and pull the fucking trigger til it goes ""click"". Vel nisi enim, vel auctor. Zere ARE no ROOLZ!",
//@"Ante morbi id urna vel. You don't go out and make a living dressed like that in the middle of a weekday. Felis lacinia placerat vestibulum turpis. Hey, relax man, I'm a brother shamus. I'll suck your cock for a thousand bucks. Nulla, viverra nec volutpat ac, Ornare id lectus cras pharetra. Mein nommen iss Karl. Is hard to verk in zese clozes. Jeffrey, you haven't gone to the doctor. Faucibus tristique nullam non accumsan.",
//@"Justo nulla facilisi integer interdum elementum nulla, nec eleifend nisl euismod. Hey! This is a private residence, man! Ac maecenas vitae eros velit, eu suscipit erat integer purus lacus, Mind if I smoke a jay? Pretium vel venenatis eu, volutpat non erat donec a metus ac. What the fuck does Vietnam have to do with anything! What the fuck were you talking about?! You think veer kidding und making mit de funny stuff? Eros dictum aliquet nulla consectetur egestas placerat maecenas pulvinar nisl et.",
//@"Nisl rhoncus at volutpat. My art has been commended as being strongly vaginal. Which bothers some men. The word itself makes some men uncomfortable. Vagina. Wonderful woman. Very free-spirited. We're all very fond of her. Felis blandit in libero. Leads, yeah. I'll just check with the boys down at the Crime Lab. They've assigned four more detectives to the case, got us working in shifts. Turpis, laoreet et molestie.",
//@"Mr. Lebowski asked me to repeat that: Her life is in your hands. Sed, volutpat et erat nulla ut. Orci quis neque consectetur tincidunt aliquam. This is quite a pad you got here, man. Completely unspoiled. Erat volutpat donec aliquam orci eget. Huh? Oh. Yeah. Tape deck. Couple of Creedence tapes. And there was a, uh… my briefcase. Mi lobortis sed tincidunt diam mattis. Hello. Nein dizbatcher says zere iss problem mit deine kable."
//                                                    };
	}
}