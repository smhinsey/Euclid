/*!
 * selectivizr v1.0.1 - (c) Keith Clark, freely distributable under the terms of the MIT license.
 * selectivizr.com
 */
var k = true, p = false;
(function(A) {

	function N(a) {
		return a.replace(O, q).replace(P, function(b, e, c) {
			b = c.split(",");
			c = 0;
			for (var g = b.length; c < g; c++) {
				var h = Q(b[c].replace(R, q).replace(S, q)) + w, f = [];
				b[c] = h.replace(T, function(d, l, m, j, i) {
					if (l) {
						if (f.length > 0) {
							d = f;
							var x;
							i = h.substring(0, i).replace(U, o);
							if (i == o || i.charAt(i.length - 1) == w) i += "*";
							try {
								x = y(i);
							} catch (ha) {
							}
							if (x) {
								i = 0;
								for (m = x.length; i < m; i++) {
									j = x[i];
									for (var B = j.className, C = 0, V = d.length; C < V; C++) {
										var r = d[C];
										if (!RegExp("(^|\\s)" + r.className + "(\\s|$)").test(j.className)) if (r.b && (r.b === k || r.b(j) === k)) B = E(B, r.className, k);
									}
									j.className = B;
								}
							}
							f = [];
						}
						return l;
					} else {
						if (l = m ? W(m) : !F || F.test(j) ? { className: G(j), b: k } : null) {
							f.push(l);
							return "." + l.className;
						}
						return d;
					}
				});
			}
			return e + b.join(",");
		});
	}

	function W(a) {
		var b = k, e = G(a.slice(1)), c = a.substring(0, 5) == ":not(", g, h;
		if (c) a = a.slice(5, -1);
		var f = a.indexOf("(");
		if (f > -1) a = a.substring(0, f);
		if (a.charAt(0) == ":")
			switch (a.slice(1)) {
			case "root":
				b = function(d) { return c ? d != H : d == H; };
				break;
			case "target":
				if (s == 8) {
					b = function(d) {

						function l() {
							var m = location.hash, j = m.slice(1);
							return c ? m == "" || d.id != j : m != "" && d.id == j;
						}

						t(A, "hashchange", function() { u(d, e, l()); });
						return l();
					};
					break;
				}
				return p;
			case "checked":
				b = function(d) {
					X.test(d.type) && t(d, "propertychange", function() { event.propertyName == "checked" && u(d, e, d.checked !== c); });
					return d.checked !== c;
				};
				break;
			case "disabled":
				c = !c;
			case "enabled":
				b = function(d) {
					if (Y.test(d.tagName)) {
						t(d, "propertychange", function() { event.propertyName == "$disabled" && u(d, e, d.a === c); });
						z.push(d);
						d.a = d.disabled;
						return d.disabled === c;
					}
					return a == ":enabled" ? c : !c;
				};
				break;
			case "focus":
				g = "focus";
				h = "blur";
			case "hover":
				if (!g) {
					g = "mouseenter";
					h = "mouseleave";
				}
				b = function(d) {
					t(d, c ? h : g, function() { u(d, e, k); });
					t(d, c ? g : h, function() { u(d, e, p); });
					return c;
				};
				break;
			default:
				if (!Z.test(a)) return p;
			}
		return { className: e, b: b };
	}

	function G(a) {
		return I + "-" + (s == 6 && $ ? aa++ : a.replace(ba, function(b) { return b.charCodeAt(0); }));
	}

	function Q(a) {
		return a.replace(J, q).replace(ca, w);
	}

	function u(a, b, e) {
		var c = a.className;
		b = E(c, b, e);
		if (b != c) {
			a.className = b;
			a.parentNode.className += o;
		}
	}

	function E(a, b, e) {
		var c = RegExp("(^|\\s)" + b + "(\\s|$)"), g = c.test(a);
		return e ? g ? a : a + w + b : g ? a.replace(c, q).replace(J, q) : a;
	}

	function t(a, b, e) {
		a.attachEvent("on" + b, e);
	}

	function D(a, b) {
		if ( /^https?:\/\//i .test(a)) return b.substring(0, b.indexOf("/", 8)) == a.substring(0, a.indexOf("/", 8)) ? a : null;
		if (a.charAt(0) == "/") return b.substring(0, b.indexOf("/", 8)) + a;
		var e = b.split("?")[0];
		if (a.charAt(0) != "?" && e.charAt(e.length - 1) != "/") e = e.substring(0, e.lastIndexOf("/") + 1);
		return e + a;
	}

	function K(a) {
		if (a) {
			v.open("GET", a, p);
			v.send();
			return (v.status == 200 ? v.responseText : o).replace(da, o).replace(ea, function(b, e, c, g, h) { return K(D(c || h, a)); }).replace(fa, function(b, e, c) {
				e = e || "";
				return " url(" + e + D(c, a) + e + ") ";
			});
		}
		return o;
	}

	function ga() {
		var a, b;
		a = n.getElementsByTagName("BASE");
		for (var e = a.length > 0 ? a[0].href : n.location.href, c = 0; c < n.styleSheets.length; c++) {
			b = n.styleSheets[c];
			if (b.href != o) if (a = D(b.href, e)) b.cssText = N(K(a));
		}
		z.length > 0 && setInterval(function() {
			for (var g = 0, h = z.length; g < h; g++) {
				var f = z[g];
				if (f.disabled !== f.a)
					if (f.disabled) {
						f.disabled = p;
						f.a = k;
						f.disabled = k;
					} else f.a = f.disabled;
			}
		}, 250);
	}

	if (!/*@cc_on!@*/true) {
		var n = document, H = n.documentElement, v = function() {
			if (A.XMLHttpRequest) return new XMLHttpRequest;
			try {
				return new ActiveXObject("Microsoft.XMLHTTP");
			} catch (a) {
				return null;
			}
		}(), s = /MSIE ([\d])/ .exec(navigator.userAgent)[1];
		if (!(n.compatMode != "CSS1Compat" || s < 6 || s > 8 || !v)) {
			var L = { NW: "*.Dom.select", DOMAssistant: "*.$", Prototype: "$$", YAHOO: "*.util.Selector.query", MooTools: "$$", Sizzle: "*", jQuery: "*", dojo: "*.query" }, y, z = [], aa = 0, $ = k, I = "slvzr", M = I + "DOMReady", da = /(\/\*[^*]*\*+([^\/][^*]*\*+)*\/)\s*/g , ea = /@import\s*(?:(?:(?:url\(\s*(['"]?)(.*)\1)\s*\))|(?:(['"])(.*)\3))[^;]*;/g , fa = /\burl\(\s*(["']?)([^"')]+)\1\s*\)/g , Z = /^:(empty|(first|last|only|nth(-last)?)-(child|of-type))$/ , O = /:(:first-(?:line|letter))/g , P = /(^|})\s*([^\{]*?[\[:][^{]+)/g , T = /([ +~>])|(:[a-z-]+(?:\(.*?\)+)?)|(\[.*?\])/g , U = /(:not\()?:(hover|enabled|disabled|focus|checked|target|active|visited|first-line|first-letter)\)?/g , ba = /[^\w-]/g , Y = /^(INPUT|SELECT|TEXTAREA|BUTTON)$/ , X = /^(checkbox|radio)$/ , F = s > 6 ? /[\$\^*]=(['"])\1/ : null, R = /([(\[+~])\s+/g , S = /\s+([)\]+~])/g , ca = /\s+/g , J = /^\s*((?:[\S\s]*\S)?)\s*$/ , o = "", w = " ", q = "$1";
			n.write("<script id=" + M + " defer src='//:'><\/script>");
			n.getElementById(M).onreadystatechange = function() {
				if (this.readyState == "complete") {
					a:{
					  	var a, b;
					  	for (b in L)
					  		if (A[b] && (a = eval(L[b].replace("*", b)))) {
					  			y = a;
					  			break a;
					  		}
					  	y = p;
					  }
					if (y) {
						ga();
						this.parentNode.removeChild(this);
					}
				}
			};
		}
	}
})(this);