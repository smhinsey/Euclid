;(function ($) {
	var app = $.sammy(function () {

		this.get('/', function () {
			$('#app-content').text('Hello');
		});

		this.get('/#company/:companySlug/financials', function () {
			
			var model = { title: "Company Financials "};

			model.body = "<i>Pretty graphs and reports go here.</i>";
			
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
			
			var model = { storeName: "{Store Name}"};

			model.body = "<i>Pretty graphs and reports go here.</i>";

			this.trigger('render-model', {templateName: 'store/Financials', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Financials'});
		});

		this.get('/#store/:storeSlug/orders', function () {

			var model = modelForStoreOrders(this.params['storeSlug']);

			this.trigger('render-model', {templateName: 'store/Orders', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Orders'});
		});
		
		this.get('/#store/:storeSlug/orders/add', function () {

			var model = modelForStoreAddOrder();

			this.trigger('render-model', {templateName: 'store/AddOrder', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Orders'});
		});

		this.get('/#store/:storeSlug/customers', function () {

			var model = modelForStoreCustomers();

			this.trigger('render-model', {templateName: 'store/Customers', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Customers'});
		});

		this.get('/#store/:storeSlug/productCatalog', function () {

			var model = modelForStoreProductCatalog();

			this.trigger('render-model', {templateName: 'store/ProductCatalog', model: model});			

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'ProductCatalog'});
		});

		this.get('/#store/:storeSlug/promotions', function() {

			var model = modelForStorePromotions();

			this.trigger('render-model', {templateName: 'store/Promotions', model: model});

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Promotions'});
		});

		this.get('/#store/:storeSlug/discountCodes', function () {

			var model = modelForStoreDiscountCodes();

			this.trigger('render-model', {templateName: 'store/DiscountCodes', model: model});

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'DiscountCodes'});
		});

	});
	
	$(function () {
		app.run();
	});
})(jQuery);

	// temporary, just for use during prototyping
	// in practice, this data would all come from queries (with the possible exception of table headers)
	
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
	
	function modelForStoreOrders(storeSlug) {

		var viewModel = { storeName: "{Store Name}", storeSlug: storeSlug };

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

		viewModel.tableHeaders = [ "Category", "Name", "For Sale", "Stock", "Price", "Discount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ category: "{Category}", name: "{Name}", forSale: true, stock: "X,XXX", price: "${XX.XX}", discount:"{X}%"});
		}

		return viewModel;
	}
	
	function modelForStorePromotions() {

		var viewModel = { storeName: "{Store Name}" };

		viewModel.tableHeaders = [ "Name", "Start Date", "End Date", "Type", "Discount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ name: "{Name}", startDate: "10/01/2012", endDate:"12/25/2012", type: "{type}", discount:"{XX}%"});
		}

		return viewModel;
	}
	
	function modelForStoreDiscountCodes() {

		var viewModel = { storeName: "{Store Name}" };

		viewModel.tableHeaders = [ "Name", "Code", "Usage Type", "Usage Count", "Discount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ name: "{Name}", code: s4() + s4(), usageType: "{Type}", usageCount:"{XX}", discount: "{X}%" });
		}

		return viewModel;
	}
	
	function modelForStoreAddOrder() {

		var viewModel = { storeName: "{Store Name}" };


		return viewModel;
	}

	function s4() {
		return (((1+Math.random())*0x10000)|0).toString(16).substring(1);
	}
	function guid() {
		return (s4()+s4()+"-"+s4()+"-"+s4()+"-"+s4()+"-"+s4()+s4()+s4());
	}
