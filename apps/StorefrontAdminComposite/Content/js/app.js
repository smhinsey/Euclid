;(function ($) {
	var app = $.sammy(function () {

		this.get('/', function () {
			$('#app-content').text('Hello');
		});

		this.get('/#company/:companySlug/financials', function () {
			
			var model = { title: "Company Financials "};

			// this approximately simulates executing a query and adding the results to the model
			model.body = "Maecenas nunc augue, fermentum ac gravida a, tempus ut neque. Nullam ut neque justo.";
			
			this.trigger('render-model', {templateName: 'company/Financials', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'Financials'});
		});

		this.get('/#company/:companySlug/allTransactions', function () {
			
			var model = { title: "Company All Transactions"};
			
			this.trigger('render-model', {templateName: 'company/AllTransactions', model: model});

			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'AllTransactions'});
		});
		
		this.get('/#company/:companySlug/employees', function () {
			
			var model = { title: "Company Employees"};
			
			this.trigger('render-model', {templateName: 'company/Employees', model: model});

			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'Employees'});
		});

		this.get('/#store/:storeSlug/financials', function () {
			
			var model = { title: "Store Financials "};

			this.trigger('render-model', {templateName: 'store/Financials', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Financials'});
		});

		this.get('/#store/:storeSlug/orders', function () {

			var model = { title: "Store Orders"};

			this.trigger('render-model', {templateName: 'store/Orders', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Orders'});
		});

		this.get('/#store/:storeSlug/customers', function () {

			var model = { title: "Store Customers"};

			this.trigger('render-model', {templateName: 'store/Customers', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Customers'});
		});

		this.get('/#store/:storeSlug/productCatalog', function () {

			var model = { title: "Store Product Catalog"};

			this.trigger('render-model', {templateName: 'store/ProductCatalog', model: model});			

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'ProductCatalog'});
		});

		this.get('/#store/:storeSlug/promotions', function() {

			var model = { title: "Store Promotions"};

			this.trigger('render-model', {templateName: 'store/Promotions', model: model});

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Promotions'});
		});

		this.get('/#store/:storeSlug/discountCodes', function () {

			var model = { title: "Store Discount Codes"};

			this.trigger('render-model', {templateName: 'store/DiscountCodes', model: model});

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'DiscountCodes'});
		});
		
		this.bind('render-model', function(e, data) {

			var templateName = data['templateName'];

			var model = data['model'];

			console.log("render-model rendering using template" + templateName);
			console.log(model);

			fetchHandlebarsTemplate('content/app/templates/' + templateName + '.handlebars', function(template) {
				var renderedOutput = template(model);
				$('#app-content').html(renderedOutput);
			});
			
		});

		this.bind('highlight-nav', function(e, data) {
			var navSelector = "#nav-" + data['slug'] + data['current'];

			$(".nav-item").removeClass("active");

			$(navSelector).addClass("active");
		});

	});
	
	function fetchHandlebarsTemplate(path, callback) {
		var source;
		var template;
 
		$.ajax({
			url: path,
			success: function(data) {
				
				source = $(data).html();

				template = Handlebars.compile(source);
				
				if (callback) {
					callback(template);
				};
			}
		});
	}

	$(function () {
		app.run();
	});
})(jQuery);