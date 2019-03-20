Cart = {
	_properties: {
		addToCartLink: "",
		getCartViewLink: "",
		removeFromCartLink: "",
		decrementLink: ""
	},
	init: function (properties)
	{
		$.extend(Cart._properties, properties);
		Cart.initEvents();
	},
	initEvents: function ()
	{
		$("a.CallAddToCart").click(Cart.addToCart);
		$("a.cart_quantity_delete").click(Cart.removeFromCart);
		$("a.cart_quantity_up").click(Cart.incrementItem);
		$("a.cart_quantity_down").click(Cart.decrementItem);
	},
	addToCart: function (event)
	{
		const button = $(this);
		event.preventDefault();
		const id = button.data("id");
		$.get(Cart._properties.addToCartLink + "/" + id)
			.done(function ()
			{
				Cart.showToolTip(button);
				Cart.refreshCartView();
			})
			.fail(function ()
			{
				console.log("addToCart error");
			});
	},
	showToolTip: function (button)
	{
		button.tooltip({ title: "Добавлено в корзину!" })
			.tooltip("show");
		setTimeout(function ()
		{
			button.tooltip("destroy");
		},
			500);
	},
	refreshCartView: function ()
	{
		const container = $("#cartContainer");
		$.get(Cart._properties.getCartViewLink)
			.done(function (result) { container.html(result); })
			.fail(function () { console.log("refreshCartView error"); });
	},
	removeFromCart: function (event)
	{
		const button = $(this);
		event.preventDefault();
		const id = button.data("id");
		$.get(Cart._properties.removeFromCartLink + "/" + id)
			.done(function ()
			{
				button.closest("tr").remove();
				Cart.refreshCartView();
			})
			.fail(function ()
			{
				console.log("removeFromCart error");
			});
	},
	incrementItem: function (event)
	{
		const button = $(this);
		const container = button.closest("tr");
		event.preventDefault();
		const id = button.data("id");
		$.get(Cart._properties.addToCartLink + "/" + id)
			.done(function ()
			{
				const value = parseInt($(".cart_quantity_input", container).val());
				$(".cart_quantity_input", container).val(value + 1);
				Cart.refreshPrice(container);
				Cart.refreshCartView();
			})
			.fail(function ()
			{
				console.log("incrementItem error");
			});
	},
	decrementItem: function (event)
	{
		const button = $(this);
		const container = button.closest("tr");
		event.preventDefault();
		const id = button.data("id");
		$.get(Cart._properties.decrementLink + "/" + id)
			.done(function ()
			{
				const value = parseInt($(".cart_quantity_input", container).val());
				if (value > 0)
				{
					$(".cart_quantity_input", container).val(value - 1);
					Cart.refreshPrice(container);
				} else
				{
					container.remove();
					Cart.refreshTotalPrice();
				}
				Cart.refreshCartView();
			})
			.fail(function ()
			{
				console.log("incrementItem error");
			});
	},
	refreshPrice: function (container)
	{
		const quantity = parseInt($(".cart_quantity_input", container).val());
		const price = parseFloat($(".cart_price", container).data("price"));
		const totalPrice = quantity * price;
		const value = totalPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });
		$(".cart_total_price", container).data("price", totalPrice);
		$(".cart_total_price", container).html(value);
		Cart.refreshTotalPrice();
	},
	refreshTotalPrice: function ()
	{
		var total = 0;
		$(".cart_total_price").each(function ()
		{
			const price = parseFloat($(this).data("price"));
			total += price;
		});
		const value = total.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });
		$("#totalOrderSum").html(value);
	}
};						