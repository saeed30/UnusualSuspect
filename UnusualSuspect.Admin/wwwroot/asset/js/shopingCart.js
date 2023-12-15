// ************************************************
// Shopping Cart API
// ************************************************

var shoppingCart = (function () {
  // =============================
  // Private methods and propeties
  // =============================
  cart = [];

  // Constructor
  function Item(name, img, price, count) {
    this.name = name;
    this.img = img;
    this.price = price;
    this.count = count;
  }

  // Save cart
  function saveCart() {
    sessionStorage.setItem('shoppingCart', JSON.stringify(cart));
  }

  // Load cart
  function loadCart() {
    cart = JSON.parse(sessionStorage.getItem('shoppingCart'));
  }
  if (sessionStorage.getItem("shoppingCart") != null) {
    loadCart();
  }


  // =============================
  // Public methods and propeties
  // =============================
  var obj = {};

  // Add to cart
  obj.addItemToCart = function (name, img, price, count) {
    for (var item in cart) {
      if (cart[item].name === name) {
        cart[item].count++;
        saveCart();
        return;
      }
    }
    var item = new Item(name, img, price, count);
    cart.push(item);
    saveCart();
  }
  // Set count from item
  obj.setCountForItem = function (name, count) {
    for (var i in cart) {
      if (cart[i].name === name) {
        cart[i].count = count;
        break;
      }
    }
  };
  // Remove item from cart
  obj.removeItemFromCart = function (name) {
    for (var item in cart) {
      if (cart[item].name === name) {
        cart[item].count--;
        if (cart[item].count === 0) {
          cart.splice(item, 1);
        }
        break;
      }
    }
    saveCart();
  }

  // Remove all items from cart
  obj.removeItemFromCartAll = function (name) {
    for (var item in cart) {
      if (cart[item].name === name) {
        cart.splice(item, 1);
        break;
      }
    }
    saveCart();
  }

  // Clear cart
  obj.clearCart = function () {
    cart = [];
    saveCart();
  }

  // Count cart 
  obj.totalCount = function () {
    var totalCount = 0;
    for (var item in cart) {
      totalCount += cart[item].count;
    }
    return totalCount;
  }

  // Total cart
  obj.totalCart = function () {
    var totalCart = 0;
    for (var item in cart) {
      totalCart += cart[item].price * cart[item].count;
    }
    return Number(totalCart.toFixed(2));
  }

  // List cart
  obj.listCart = function () {
    var cartCopy = [];
    for (i in cart) {
      item = cart[i];
      itemCopy = {};
      for (p in item) {
        itemCopy[p] = item[p];

      }
      itemCopy.total = Number(item.price * item.count).toFixed(2);
      cartCopy.push(itemCopy)
    }
    return cartCopy;
  }

  // cart : Array
  // Item : Object/Class
  // addItemToCart : Function
  // removeItemFromCart : Function
  // removeItemFromCartAll : Function
  // clearCart : Function
  // countCart : Function
  // totalCart : Function
  // listCart : Function
  // saveCart : Function
  // loadCart : Function
  return obj;
})();


// *****************************************
// Triggers / Events
// ***************************************** 
// Add item
$('.add-to-cart').click(function (event) {
  event.preventDefault();
  var name = $(this).data('name');
  var img = $('.add-to-cart').attr('data-source');
  var price = Number($(this).data('price'));
  shoppingCart.addItemToCart(name, img, price, 1);
  displayCart();
  $('.empty-basket').hide();
  $('.badge').removeClass('hide').addClass('show');

  var sum = 0;
  $(".total-price input").each(function () {
    var p = $(this).val();
    if (p) {
      sum += parseInt(p);
    }
  });
  $('.final-price').text(sum);
});

// Clear items
$('.clear-cart').click(function () {
  shoppingCart.clearCart();
  displayCart();
  $('.empty-basket').show();
  $('.final-price').text('-');
  $('.badge').removeClass('show').addClass('hide');
});


$(document).ready(function () {
  if (parseInt($('.total-count').text()) < 1) {
    $('.badge').removeClass('show').addClass('hide');
  } else {
    $('.badge').removeClass('hide').addClass('show');
  }
});


function displayCart() {
  var cartArray = shoppingCart.listCart();
  var output = "";
  for (var i in cartArray) {
    output += "<div class='basket-cart'>"
      + "<div><img class='basket-img' src='" + cartArray[i].img + "'/></div>"
      + "<div><div>" + cartArray[i].name + "</div>"
      + "<div>" + 'قیمت کالا: ' + cartArray[i].price + "</div>"
      + "<div><div class='item-count'> <button type='button' class='plus icon-feather-plus input-group-addon' data-name='" + cartArray[i].name + "'></button>"
      + "<span class='item-count__number' " + cartArray[i].name + ">" + cartArray[i].count + "</span>"
      + "<button type='button' class='minus input-group-addon icon-feather-trash-2' data-name='" + cartArray[i].name + "'></button></div></div>"
      + "<button type='button' class='delete-item icon-feather-x' data-name='" + cartArray[i].name + "'></button>"
      + "<div class='total-price'>" + 'مجموع قیمت: ' + '<input value=' + parseInt(cartArray[i].total) + ' readonly>' + "</div></div>"
      + "</div>";
  }
  $('.show-cart').html(output);
  $('.total-cart').html(shoppingCart.totalCart());
  $('.total-count').html(shoppingCart.totalCount());
}

// Delete item button

$('.show-cart').on("click", ".delete-item", function (event) {
  var name = $(this).data('name')
  shoppingCart.removeItemFromCartAll(name);
  displayCart();
  $('.empty-basket').show();
  $('.badge').removeClass('show').addClass('hide');
  var sum = 0;
  $(".total-price input").each(function () {
    var p = $(this).val();
    if (p) {
      sum += parseInt(p);
    }
  });
  $('.final-price').text(sum);
})


// -1
$('.show-cart').on("click", ".minus", function (event) {
  var name = $(this).data('name')
  shoppingCart.removeItemFromCart(name);

  displayCart();
  if (parseInt($('.total-count').text()) < 1) {
    $('.badge').removeClass('show').addClass('hide');
    $('.empty-basket').show();
  }

  var sum = 0;
  $(".total-price input").each(function () {
    var p = $(this).val();
    if (p) {
      sum += parseInt(p);
    }
  });
  $('.final-price').text(sum);
})
// +1
$('.show-cart').on("click", ".plus", function (event) {
  var name = $(this).data('name')
  console.log($(this).data('name'));
  shoppingCart.addItemToCart(name);

  displayCart();

  var sum = 0;
  $(".total-price input").each(function () {
    var p = $(this).val();
    if (p) {
      sum += parseInt(p);
    }
  });

  $('.final-price').text(sum);
})

// Item count input
$('.show-cart').on("change", ".item-count", function (event) {
  var name = $(this).data('name');
  var count = Number($(this).val());
  shoppingCart.setCountForItem(name, count);
  displayCart();
});

displayCart();

$(document).ready(function () {
  if ($('.basket-cart').length > 0) {
    $('.empty-basket').hide();
  }
});



