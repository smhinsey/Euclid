function setForumUrl(host, orgSlug, forumSlug) {
	var url = "http://" + host + "/org/" + orgSlug + "/forum/" + forumSlug;

	$("#forum-url").html(url);
}

function isNullOrEmpty(value) {
	return value == null || value == "";
}

function slugify(value) {
	return value.replace(/ /g, "-").toLowerCase();
}

$.fn.selectRange = function (start, end) {
	return this.each(function () {
		if (this.setSelectionRange) {
			this.focus();
			this.setSelectionRange(start, end);
		} else if (this.createTextRange) {
			var range = this.createTextRange();
			range.collapse(true);
			range.moveEnd('character', end);
			range.moveStart('character', start);
			range.select();
		}
	});
};