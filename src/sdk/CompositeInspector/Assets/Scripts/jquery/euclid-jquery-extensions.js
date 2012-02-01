// extension methods
$.fn.hasAttr = function (name) {
	///<summary>easily check if an element contains an attribute<summary>
	///<param name='name'>The name of the attribute to check for</param>
	return this.attr(name) !== undefined;
};

if (jQuery.validator != null) {
	jQuery.validator.addMethod('uniquevalue', function (value, element, params) {
		var query = $(element).attr("data-val-uniquevalue-query");
		var method = $(element).attr("data-val-uniquevalue-method");
		var argument = $(element).attr("data-val-uniquevalue-argument");
		var argumentObject = $.parseJSON("{ \"" + argument + "\": \"" + value + "\"}")
		var results = EUCLID.executeQuery({ queryName: query, methodName: method, parameters: argumentObject });

		return results == null;
	}, '');

	// and an unobtrusive adapter
	jQuery.validator.unobtrusive.adapters.add('uniquevalue', {}, function (options) {
		options.rules['uniquevalue'] = true;
		options.messages['uniquevalue'] = options.message;
	});
}