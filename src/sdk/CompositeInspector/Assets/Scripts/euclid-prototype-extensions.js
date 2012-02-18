String.prototype.endsWith = function (suffix) {
	///<summary>returns true if the string ends with the specified suffix (case sensitive)</summary>
	///<param name='suffix'>the string the check for</param>
	return this.indexOf(suffix, this.length - suffix.length) !== -1;
};

if (typeof String.prototype.startsWith != 'function') {
	String.prototype.startsWith = function (str) {
		return this.indexOf(str) == 0;
	};
}