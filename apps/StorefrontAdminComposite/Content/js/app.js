;(function ($) {
	var app = $.sammy(function () {

		this.get('/', function () {
			$('#app-content').text('Hello');
		});

		this.get('/#company/:companySlug/financials', function () {
			$('#app-content').text('Company Financials');
			
			this.trigger('set-nav', {slug: this.params['companySlug'], current: 'Financials'});
		});

		this.get('/#company/:companySlug/allTransactions', function () {
			$('#app-content').text('Company All Transactions');
			
			this.trigger('set-nav', {slug: this.params['companySlug'], current: 'AllTransactions'});
		});
		
		this.get('/#company/:companySlug/employees', function () {
			$('#app-content').text('Company Employees');
			
			this.trigger('set-nav', {slug: this.params['companySlug'], current: 'Employees'});
		});

		this.get('/#store/:storeSlug/financials', function () {
			$('#app-content').text('Store Financials');
			
			this.trigger('set-nav', {slug: this.params['storeSlug'], current: 'Financials'});
		});

		this.get('/#store/:storeSlug/orders', function () {
			$('#app-content').text('Store Orders');
			
			this.trigger('set-nav', {slug: this.params['storeSlug'], current: 'Orders'});
		});

		this.get('/#store/:storeSlug/customers', function () {
			$('#app-content').text('Store Customers');
			
			this.trigger('set-nav', {slug: this.params['storeSlug'], current: 'Customers'});
		});

		this.get('/#store/:storeSlug/productCatalog', function () {
			$('#app-content').text('Store Product Catalog');
			
			this.trigger('set-nav', {slug: this.params['storeSlug'], current: 'ProductCatalog'});
		});

		this.get('/#store/:storeSlug/promotions', function() {
			$('#app-content').text('Store Promotions');

			this.trigger('set-nav', {slug: this.params['storeSlug'], current: 'Promotions'});
		});

		this.get('/#store/:storeSlug/discountCodes', function () {
			$('#app-content').text('Store Discount Codes');

			this.trigger('set-nav', {slug: this.params['storeSlug'], current: 'DiscountCodes'});
		});
		
		this.bind('set-nav', function(e, data) {
			var navSelector = "#nav-" + data['slug'] + data['current'];

			$(".nav-item").removeClass("active");

			$(navSelector).addClass("active");
		});

	});

	$(function () {
		app.run();
	});
})(jQuery);