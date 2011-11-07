$(document).ready(function () {
	if (!$("#users-nav-item").parent().hasClass("current")) {
		$($("#users-nav-item").siblings()[0]).show();
	}

	if (!$("#forum-nav-item").parent().hasClass("current")) {
		$($("#forum-nav-item").siblings()[0]).show();
	}

	$("#forum-name").live("keyup", function () {
		setForumUrl();
	});

	$("#forum-host").live("keyup", function () {
		setForumUrl();
	});

	$("#theme-selector").live("change", function () {
		var src = getPreviewImage($(this).val());

		if (src == "") {
			$("#unselected-preview").show();
			$("#theme-preview").hide();
		}
		else {
			$("#unselected-preview").hide();
			$("#theme-preview").show();
			$("#theme-preview").attr("src", src);
		}
	});

	$("#new-forum-button").click(function () {
		$("<div></div>")
			.load("/Forum/NewForum")
			.dialog({
				modal: true,
				title: "New Forum",
				width: 750,
				height: 800
			});

		return false;
	});

	$(".vote-scheme").live("click", function () {
		var description = "";

		if ($(this).val() == "00000000-0000-0000-0000-000000000000") {
			description = "Voting disabled";
		} else if ($(this).val() == "9A52AFD8-196B-475D-94DE-EC15F9C8E367") {
			description = "Users can vote Up (+1) or Down (-1)";
		}

		$("#vote-scheme-description").text(description);
	});

	$(".activate-badge").click(function () {

	});
});
	
function setForumUrl() {
	var forumSlug = getForumSlug();

	var host = slugify($("#forum-host").val());
	$("#host-example").html(host);

	var url;
	if (host == "@Model.UrlHostName") {
		url = "http://" + host + "/{organization name}/forums/" + forumSlug;
	} else {
		url = "http://" + host + "/forums/" + getForumSlug();
	}

	$("#forum-url").html(url);
}

function getForumSlug() {
	var forumSlug = $("#forum-name").val();

	if (forumSlug == "" || forumSlug == null) {
		forumSlug = "{forum-name}";
	}
	else {
		forumSlug = slugify($("#forum-name").val());
		$("#forum-slug").val(forumSlug);
	}

	return forumSlug;
}

function getPreviewImage(id) {
	var imageUrl;
	if (id == "794B96E8-83C9-4F2F-A02A-45E8BCF1D765") {
		imageUrl = "http://www.vbstyles.com/vbulletin_styles/l_RedFox-4x-vBulletin-Theme-Forum-Home1.png";
	} else if (id == "58B560E0-EFE6-400C-A882-A7A4B18C4FCC") {
		imageUrl = "http://themes.wordpress-deutschland.org/demoblog/wp-content/themes/insomniac/screenshot.png";
	} else {
		imageUrl = "";
	}
	return imageUrl;
}

function slugify(value) {
	return value.replace(/ /g, "-").toLowerCase();
}