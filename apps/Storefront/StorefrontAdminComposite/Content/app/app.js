;(function ($) {
	var app = $.sammy(function () {

		this.get('/#sysadmin/dashboard', function () {

			var model = modelForSysAdminDashboard();
			
			this.trigger('render-model', {templateName: 'sysadmin/Dashboard', model: model});
			
			this.trigger('highlight-nav', {slug: 'SysAdmin', current: 'Dashboard'});
		});

		this.get('/#sysadmin/settings', function () {

			var model = modelForSysAdminSettings();
			
			this.trigger('render-model', {templateName: 'sysadmin/Settings', model: model});
			
			this.trigger('highlight-nav', {slug: 'SysAdmin', current: 'Settings'});
		});
		
		this.get('/#sysadmin/companies/add', function () {

			var model = modelForSysAdminAddCompany();
			
			this.trigger('render-model', {templateName: 'sysadmin/AddCompany', model: model});
			
			this.trigger('highlight-nav', {slug: 'SysAdmin', current: 'AddCompany'});
		});
		
		this.get('/#sysadmin/administrators', function () {

			var model = modelForSysAdminAdministrators();
			
			this.trigger('render-model', {templateName: 'sysadmin/Administrators', model: model});
			
			this.trigger('highlight-nav', {slug: 'SysAdmin', current: 'Administrators'});
		});
		
		this.get('/#sysadmin/administrators/add', function () {

			var model = modelForSysAdminAddAdministrator();
			
			this.trigger('render-model', {templateName: 'sysadmin/AddAdministrator', model: model});
			
			this.trigger('highlight-nav', {slug: 'SysAdmin', current: 'Administrators'});
		});

		this.get('/#company/:companySlug/financials', function () {
			
			var model = { title: "{Company Name} Financials "};

			model.body = "<i>Pretty graphs and reports go here.</i>";
			
			this.trigger('render-model', {templateName: 'company/Financials', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'Financials'});
		});

		this.get('/#company/:companySlug/allTransactions', function () {
			
			var model = modelForCompanyAllTransactions(this.params['companySlug']);
			
			this.trigger('render-model', {templateName: 'company/AllTransactions', model: model});

			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'AllTransactions'});
		});
		
		this.get('/#company/:companySlug/allTransactions/:transactionSlug/details', function () {
			
			var model = modelForCompanyTransactionDetails(this.params['companySlug']);
			
			this.trigger('render-model', {templateName: 'company/TransactionDetails', model: model});

			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'AllTransactions'});
		});
		
		this.get('/#company/:companySlug/employees', function () {
			
			var model = modelForCompanyEmployees(this.params['companySlug']);
			
			this.trigger('render-model', {templateName: 'company/Employees', model: model});

			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'Employees'});
		});
		
		this.get('/#company/:companySlug/employees/:employeeSlug/details', function () {
			
			var model = modelForCompanyEmployeeDetails(this.params['companySlug']);
			
			this.trigger('render-model', {templateName: 'company/EmployeeDetails', model: model});

			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'Employees'});
		});
		
		this.get('/#company/:companySlug/employees/add', function () {
			
			var model = modelForCompanyEmployees();
			
			this.trigger('render-model', {templateName: 'company/AddEmployee', model: model});

			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'Employees'});
		});
		
		this.get('/#company/:companySlug/newStore', function () {
			
			var model = modelForCompanyNewStore();
			
			this.trigger('render-model', {templateName: 'company/NewStore', model: model});

			this.trigger('highlight-nav', {slug: this.params['companySlug'], current: 'NewStore'});
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

			var model = modelForStoreCustomers(this.params['storeSlug']);

			this.trigger('render-model', {templateName: 'store/Customers', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Customers'});
		});
		
		this.get('/#store/:storeSlug/customers/:customerSlug/profile', function () {

			var model = modelForStoreCustomerProfile(this.params['storeSlug']);

			this.trigger('render-model', {templateName: 'store/CustomerProfile', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Customers'});
		});
		
		this.get('/#store/:storeSlug/customers/:customerSlug/reset', function () {

			var model = modelForStoreCustomerReset(this.params['storeSlug']);

			this.trigger('render-model', {templateName: 'store/CustomerReset', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Customers'});
		});
		
		this.get('/#store/:storeSlug/customers/:customerSlug/contact', function () {

			var model = modelForStoreCustomerContact(this.params['storeSlug']);

			this.trigger('render-model', {templateName: 'store/CustomerContact', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Customers'});
		});
		
		this.get('/#store/:storeSlug/customers/invite', function () {

			var model = modelForStoreInviteCustomer();

			this.trigger('render-model', {templateName: 'store/InviteCustomer', model: model});
			
			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Customers'});
		});

		this.get('/#store/:storeSlug/productCatalog', function () {

			var model = modelForStoreProductCatalog(this.params['storeSlug']);

			this.trigger('render-model', {templateName: 'store/ProductCatalog', model: model});			

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'ProductCatalog'});
		});
		
		this.get('/#store/:storeSlug/productCatalog/:productSlug/details', function () {

			var model = modelForStoreProductDetails(this.params['storeSlug']);

			this.trigger('render-model', {templateName: 'store/ProductDetails', model: model});			

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'ProductCatalog'});
		});
		
		this.get('/#store/:storeSlug/productCatalog/add', function () {

			var model = modelForStoreAddProduct();

			this.trigger('render-model', {templateName: 'store/AddProduct', model: model});			

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'ProductCatalog'});
		});

		this.get('/#store/:storeSlug/promotions', function() {

			var model = modelForStorePromotions(this.params['storeSlug']);

			this.trigger('render-model', {templateName: 'store/Promotions', model: model});

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Promotions'});
		});
		
		this.get('/#store/:storeSlug/promotions/:promotionSlug/details', function() {

			var model = modelForStorePromotionDetails(this.params['storeSlug']);

			this.trigger('render-model', {templateName: 'store/PromotionDetails', model: model});

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Promotions'});
		});
		
		this.get('/#store/:storeSlug/promotions/add', function() {

			var model = modelForStoreAddPromotion();

			this.trigger('render-model', {templateName: 'store/AddPromotion', model: model});

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'Promotions'});
		});

		this.get('/#store/:storeSlug/discountCodes', function () {

			var model = modelForStoreDiscountCodes(this.params['storeSlug']);

			this.trigger('render-model', {templateName: 'store/DiscountCodes', model: model});

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'DiscountCodes'});
		});
		
		this.get('/#store/:storeSlug/discountCodes/:discountCodeSlug/details', function () {

			var model = modelForStoreDiscountCodeDetails(this.params['storeSlug']);

			this.trigger('render-model', {templateName: 'store/DiscountCodeDetails', model: model});

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'DiscountCodes'});
		});
		
		this.get('/#store/:storeSlug/discountCodes/add', function () {

			var model = modelForStoreAddDiscountCode();

			this.trigger('render-model', {templateName: 'store/AddDiscountCode', model: model});

			this.trigger('highlight-nav', {slug: this.params['storeSlug'], current: 'DiscountCodes'});
		});

		this.get('/', function () {
			this.redirect('#sysadmin/dashboard');
		});

	});
	
	$(function () {
		app.run();
	});
})(jQuery);

	// temporary, just for use during prototyping
	// in practice, this data would all come from queries (with the possible exception of table headers)
	
	function modelForSysAdminDashboard() {

		var viewModel = { title: "Dashboard" };

		return viewModel;
	}

	function modelForSysAdminSettings() {

		var viewModel = { title: "Settings" };

		return viewModel;
	}
	
	function modelForSysAdminAddCompany() {

		var viewModel = { title: "Add Company" };

		return viewModel;
	}
	
	function modelForSysAdminAdministrators() {

		var viewModel = { title: "Administrators" };
		
		viewModel.tableHeaders = [ "Last Name", "First Name", "Email", "Status", "Last Seen", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ lastName: "{Last Name}", firstName: "{First Name}", email:"{email@email.com}", status:"{Status}", lastSeen: "12/25/2012 12:55 PM ET"});
		}

		return viewModel;
	}
	
	function modelForSysAdminAddAdministrator() {

		var viewModel = { title: "Add Administrator" };
		
		return viewModel;
	}

	function modelForCompanyAllTransactions(companySlug) {

		var viewModel = { companyName: "{Company Name}", companySlug: companySlug};

		viewModel.tableHeaders = [ "Id", "State", "Date/Time", "Type", "Amount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ id: s4()+s4(), dateTime: "12/25/2012 12:55 PM ET", type: "{Type}", storeName: "{Store Name}", amount: "{Amount}", state: "{State}"});
		}

		return viewModel;
	}
	
	function modelForCompanyTransactionDetails() {

		var viewModel = { companyName: "{Company Name}", transactionId: "{Transaction ID}" };

		return viewModel;
	}

	function modelForCompanyEmployees(companySlug) {

		var viewModel = { companyName: "{Company Name}", companySlug: companySlug };

		viewModel.tableHeaders = [ "Last Name", "First Name", "Type", "Hire Date", "Location", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ firstName: "{First}", lastName: "{Last}", type: "{Type}", hireDate: "12/25/2012", location: "{Location}"});
		}

		return viewModel;
	}
	
	function modelForCompanyEmployeeDetails(companySlug) {

		var viewModel = { companyName: "{Company Name}", companySlug: companySlug };

		return viewModel;
	}
	
	function modelForCompanyNewStore(companySlug) {

		var viewModel = { companyName: "{Company Name}", companySlug: companySlug };

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

	function modelForStoreCustomers(storeSlug) {

		var viewModel = { storeName: "{Store Name}", storeSlug: storeSlug };

		viewModel.tableHeaders = [ "Email", "User Name", "Sign up Date", "Purchase Total", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ email: "{email}", username: "{username}", signupDate: "12/25/2012", totalPurchaseAmount: "$X,XXX.00"});
		}

		return viewModel;
	}
	
	function modelForStoreCustomerProfile(storeSlug) {

		var viewModel = { storeName: "{Store Name}", storeSlug: storeSlug };

		return viewModel;
	}
	
	function modelForStoreCustomerReset(storeSlug) {

		var viewModel = { storeName: "{Store Name}", storeSlug: storeSlug };

		return viewModel;
	}
	
	function modelForStoreCustomerContact(storeSlug) {

		var viewModel = { storeName: "{Store Name}", storeSlug: storeSlug };

		return viewModel;
	}

	function modelForStoreProductCatalog(storeSlug) {

		var viewModel = { storeName: "{Store Name}", storeSlug: storeSlug };

		viewModel.tableHeaders = [ "Category", "Name", "For Sale", "Stock", "Price", "Discount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ category: "{Category}", name: "{Name}", forSale: true, stock: "X,XXX", price: "${XX.XX}", discount:"{X}%"});
		}

		return viewModel;
	}
	
	function modelForStoreProductDetails(storeSlug) {

		var viewModel = { storeName: "{Store Name}", storeSlug: storeSlug };

		viewModel.tableHeaders = [ "Category", "Name", "For Sale", "Stock", "Price", "Discount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ category: "{Category}", name: "{Name}", forSale: true, stock: "X,XXX", price: "${XX.XX}", discount:"{X}%"});
		}

		return viewModel;
	}
	
	function modelForStorePromotions(storeSlug) {

		var viewModel = { storeName: "{Store Name}", storeSlug: storeSlug };

		viewModel.tableHeaders = [ "Name", "Start Date", "End Date", "Type", "Discount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ name: "{Name}", startDate: "10/01/2012", endDate:"12/25/2012", type: "{type}", discount:"{XX}%"});
		}

		return viewModel;
	}
	
	function modelForStorePromotionDetails(storeSlug) {

		var viewModel = { storeName: "{Store Name}", storeSlug: storeSlug };

		return viewModel;
	}
	
	function modelForStoreDiscountCodes(storeSlug) {

		var viewModel = { storeName: "{Store Name}", storeSlug: storeSlug };

		viewModel.tableHeaders = [ "Name", "Code", "Usage Type", "Usage Count", "Discount", ""];
		
		viewModel.tableData = [];

		for (var i = 0; i < 10; i++) {
			viewModel.tableData.push({ name: "{Name}", code: s4() + s4(), usageType: "{Type}", usageCount:"{XX}", discount: "{X}%" });
		}

		return viewModel;
	}
	
	function modelForStoreDiscountCodeDetails(storeSlug) {

		var viewModel = { storeName: "{Store Name}", storeSlug: storeSlug };

		return viewModel;
	}
	
	function modelForStoreAddOrder() {

		var viewModel = { storeName: "{Store Name}" };

		return viewModel;
	}

	function modelForStoreInviteCustomer() {

		var viewModel = { storeName: "{Store Name}" };

		return viewModel;
	}
	
	function modelForStoreAddProduct() {

		var viewModel = { storeName: "{Store Name}" };

		return viewModel;
	}
	
	function modelForStoreAddPromotion() {

		var viewModel = { storeName: "{Store Name}" };

		return viewModel;
	}
	
	function modelForStoreAddDiscountCode() {

		var viewModel = { storeName: "{Store Name}" };

		return viewModel;
	}

	function s4() {
		return (((1+Math.random())*0x10000)|0).toString(16).substring(1);
	}
	function guid() {
		return (s4()+s4()+"-"+s4()+"-"+s4()+"-"+s4()+"-"+s4()+s4()+s4());
	}
