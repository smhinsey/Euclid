String.prototype.endsWith = function (suffix) {
	///<summary>returns true if the string ends with the specified suffix (case sensitive)</summary>
	///<param name='suffix'>the string the check for</param>
	return this.indexOf(suffix, this.length - suffix.length) !== -1;
};
