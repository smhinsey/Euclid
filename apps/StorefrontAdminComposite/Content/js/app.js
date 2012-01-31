﻿;(function ($) {
	var app = $.sammy(function () {

		this.get('/', function () {
			$('#app-content').text('Hello');
		});

		this.get('/#company/:companySlug/financials', function () {
			
			var model = { title: "Company Financials "};

			model.body = "(Coming soon.)";
			
			this.trigger('render-model', {templateName: 'company/Financials', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'Financials'});
		});

		this.get('/#company/:companySlug/allTransactions', function () {
			
			var model = modelForCompanyAllTransactions();
			
			this.trigger('render-model', {templateName: 'company/AllTransactions', model: model});

			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'AllTransactions'});
		});
		
		this.get('/#company/:companySlug/employees', function () {
			
			var model = modelForCompanyEmployees();
			
			this.trigger('render-model', {templateName: 'company/Employees', model: model});

			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'Employees'});
		});

		this.get('/#store/:storeSlug/financials', function () {
			
			var model = { title: "Store Financials "};

			this.trigger('render-model', {templateName: 'store/Financials', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Financials'});
		});

		this.get('/#store/:storeSlug/orders', function () {

			var model = modelForStoreOrders();

			this.trigger('render-model', {templateName: 'store/Orders', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Orders'});
		});

		this.get('/#store/:storeSlug/customers', function () {

			var model = modelForStoreCustomers();

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
	
	// temporary, just for use during prototyping
	
	function modelForCompanyAllTransactions() {

		var viewModel = { companyName: "{Company Name}" };

		viewModel.tableHeaders = [ "Id", "State", "Date/Time", "Type", "Amount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ id: s4()+s4(), dateTime: "12/25/2012 12:55 PM ET", type: "{Type}", storeName: "{Store Name}", amount: "{Amount}", state: "{State}"});
		}

		return viewModel;
	}

	function modelForCompanyEmployees() {

		var viewModel = { companyName: "{Company Name}" };

		viewModel.tableHeaders = [ "Last Name", "First Name", "Type", "Hire Date", "Location", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ firstName: "{First}", lastName: "{Last}", type: "{Type}", hireDate: "12/25/2012", location: "{Location}"});
		}

		return viewModel;
	}
	
	function modelForStoreOrders() {

		var viewModel = { storeName: "{Store Name}" };

		viewModel.tableHeaders = [ "Id", "Date/Time", "Type", "Amount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ id: s4()+s4(), dateTime: "12/25/2012 12:55 PM ET", type: "{Type}", storeName: "{Store Name}", amount: "{Amount}"});
		}

		return viewModel;
	}

	function modelForStoreCustomers() {

		var viewModel = { storeName: "{Store Name}" };

		viewModel.tableHeaders = [ "Email", "User Name", "Sign up Date", "Purchase Total", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ email: "{email}", username: "{username}", signupDate: "12/25/2012", totalPurchaseAmount: "$X,XXX.00"});
		}

		return viewModel;
	}
	
	function modelForStoreProductCatalog() {

		var viewModel = { storeName: "{Store Name}" };

		viewModel.tableHeaders = [ "Id", "Date/Time", "Type", "Amount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ id: s4()+s4(), dateTime: "12/25/2012 12:55 PM ET", type: "{Type}", storeName: "{Store Name}", amount: "{Amount}"});
		}

		return viewModel;
	}
	
	function modelForStorePromotions() {

		var viewModel = { storeName: "{Store Name}" };

		viewModel.tableHeaders = [ "Id", "Date/Time", "Type", "Amount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ id: s4()+s4(), dateTime: "12/25/2012 12:55 PM ET", type: "{Type}", storeName: "{Store Name}", amount: "{Amount}"});
		}

		return viewModel;
	}
	
	function modelForStoreDiscountCodes() {

		var viewModel = { storeName: "{Store Name}" };

		viewModel.tableHeaders = [ "Id", "Date/Time", "Type", "Amount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ id: s4()+s4(), dateTime: "12/25/2012 12:55 PM ET", type: "{Type}", storeName: "{Store Name}", amount: "{Amount}"});
		}

		return viewModel;
	}

	function s4() {
		return (((1+Math.random())*0x10000)|0).toString(16).substring(1);
	}
	function guid() {
		return (s4()+s4()+"-"+s4()+"-"+s4()+"-"+s4()+"-"+s4()+s4()+s4());
	}

	$(function () {
		app.run();
	});
})(jQuery);