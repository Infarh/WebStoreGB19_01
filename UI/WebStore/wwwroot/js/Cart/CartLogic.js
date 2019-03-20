Cart = {
	_properties: {
		addToCartLink: "",
		getCartViewLink: ""
	},
	init: function (properties)
	{
		$.extend(Cart._properties, properties);
		Cart.initAddToCart();
	},
	initAddToCart: function ()
	{
		$("a.CallAddToCart").click(Cart.addToCart);
	},
	addToCart: function (event)
	{
		const button = $(this);
		event.preventDefault();
		const id = button.data("id");
		$.get(Cart._properties.addToCartLink + "/" + id)
			.done(function () {
				Cart.showToolTip(button);
				Cart.refreshCartView();
			})
			.fail(function ()
			{
				console.log("addToCart error");
			});
	},
	showToolTip: function(button) {
		button.tooltip({ title: "Добавлено в корзину!" })
			.tooltip("show");
		setTimeout(function() {
				button.tooltip("destroy");
			},
			500);
	},
	refreshCartView: function() {
		const container = $("#cartContainer");
		$.get(Cart._properties.getCartViewLink)
			.done(function (result) { container.html(result); })
			.fail(function () { console.log("refreshCartView error"); });
	}
};						