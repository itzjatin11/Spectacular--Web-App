document.addEventListener("DOMContentLoaded", function () {
    loadProducts();
});

function loadProducts() {
    fetch('/Admin/GetProducts')
        .then(response => response.json())
        .then(products => {
            let tableBody = document.getElementById("productTableBody");
            tableBody.innerHTML = "";
            products.forEach(product => {
                let row =
                    <tr>
                        <td>${product.id}</td>
                        <td>${product.name}</td>
                        <td>$${product.price.toFixed(2)}</td>
                        <td><img src="${product.imageUrl}" alt="Product Image" width="50"></td>
                        <td>${product.category}</td>
                        <td>
                            <button class="btn btn-warning btn-sm" onclick="editProduct(${product.id}, '${product.name}', ${product.price}, '${product.imageUrl}', '${product.category}')">Edit</button>
                            <button class="btn btn-danger btn-sm" onclick="deleteProduct(${product.id})">Delete</button>
                        </td>
                    </tr>;

                tableBody.innerHTML += row;
            });
        });
}

function addProduct() {
    let name = document.getElementById("newProductName").value;
    let price = document.getElementById("newProductPrice").value;
    let imageUrl = document.getElementById("newProductImageUrl").value;
    let category = document.getElementById("newProductCategory").value;

    fetch('/Admin/AddProduct', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, price, imageUrl, category })
    }).then(() => loadProducts());
}

function deleteProduct(id) {
    if (confirm("Are you sure you want to delete this product?")) {
        fetch(`/Admin/DeleteProduct/${id}`, { method: 'DELETE' })
            .then(() => loadProducts());
    }
}

function editProduct(id, name, price, imageUrl, category) {
    document.getElementById("editProductId").value = id;
    document.getElementById("editProductName").value = name;
    document.getElementById("editProductPrice").value = price;
    document.getElementById("editProductImageUrl").value = imageUrl;
    document.getElementById("editProductCategory").value = category;
    document.getElementById("editProductModal").style.display = "block";
}

function saveEditProduct() {
    let id = document.getElementById("editProductId").value;
    let name = document.getElementById("editProductName").value;
    let price = document.getElementById("editProductPrice").value;
    let imageUrl = document.getElementById("editProductImageUrl").value;
    let category = document.getElementById("editProductCategory").value;

    fetch('/Admin/EditProduct', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ id, name, price, imageUrl, category })
    }).then(() => {
        document.getElementById("editProductModal").style.display = "none";
        loadProducts();
    });
}
