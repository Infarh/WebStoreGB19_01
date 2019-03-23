ProductItems = {
	_options: {
		getUrl: ""
	},
	init: function (options)
	{
		$.extend(ProductItems._options, options);
		$(".pagination li a").click(ProductItems.clickOnPage);
	},
	clickOnPage: function (event)
	{
		event.preventDefault();

		if ($(this).prop("href").length > 0) {
			var page = $(this).data("page");
			$("#itemsContainer").LoadingOverlay("show");

			var data = $(this).data();

			var query = "";

			for (var key in data) {
				if (data.hasOwnProperty(key)) {
					query += key + "=" + data[key] + "&";
				}
			}

			$.get(ProductItems._options.getUrl + "?" + query)
				.done(function(result) {
					$("#itemsContainer").html(result);
					$("#itemsContainer").LoadingOverlay("hide");

					$(".pagination li").removeClass("active");
					$(".pagination li a").prop("href", "#");
					$(".pagination li a[data-page=" + page + "]")
						.removeAttr("href")
						.parent()
						.addClass("active");

				})
				.fail(function() {
					console.log("clickOnPage getItems error");
					$("#itemsContainer").LoadingOverlay("hide");
				});
		}
	}
};