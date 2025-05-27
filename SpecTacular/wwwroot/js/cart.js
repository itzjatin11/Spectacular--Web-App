if (!window.cartInitialized) {
    window.cartInitialized = true;

    function updateCartCount() {
        let cart = JSON.parse(localStorage.getItem("cart")) || [];
        let count = cart.reduce((sum, item) => sum + item.quantity, 0);
        let cartCountElement = document.querySelector("#cart-count");
        if (cartCountElement) {
            cartCountElement.textContent = count;
        }

        document.cookie = "cart=" + JSON.stringify(cart) + "; path=/; max-age=604800";
    }

    document.removeEventListener("click", handleAddToCart);
    function handleAddToCart(event) {
        if (event.target.classList.contains("add-to-cart")) {
            event.preventDefault();
            event.stopPropagation();
            console.log("Add to Cart clicked");

            let button = event.target;
            let product = {
                name: button.getAttribute("data-name"),
                price: parseFloat(button.getAttribute("data-price")),
                quantity: 1
            };

            let cart = JSON.parse(localStorage.getItem("cart")) || [];
            let existingItem = cart.find(item => item.name === product.name);
            if (existingItem) {
                existingItem.quantity += 1;
            } else {
                cart.push(product);
            }

            localStorage.setItem("cart", JSON.stringify(cart));
            updateCartCount();
            alert(product.name + " added to cart!");
        }
    }

    document.addEventListener("click", handleAddToCart);

    document.addEventListener("DOMContentLoaded", function () {
        updateCartCount();
    });
}