document.addEventListener("DOMContentLoaded", function () {
    loadProducts();

    // Wire the Save & Cancel buttons
    document.getElementById("saveBtn").addEventListener("click", saveProduct);
    document.getElementById("cancelBtn").addEventListener("click", closeAddModal);

    document.getElementById("deleteYesBtn").addEventListener("click", deleteProduct);
    document.getElementById("deleteCancelBtn").addEventListener("click", closeDeleteConfirmationModal);
});

let isEditMode = false;
let productToDelete = null;

function openAddModal() {
    isEditMode = false;
    clearAddForm();
    document.getElementById("pcode").disabled = false; // enable for Add
    document.getElementById("addProductModal").style.display = "flex";
}

function closeAddModal() {
    clearAddForm();
    document.getElementById("addProductModal").style.display = "none";
}

function openDeleteConfirmationModal(productCode) {

    productToDelete = productCode;
    document.getElementById("deleteConfirmModal").style.display = "flex";
}

function closeDeleteConfirmationModal() {
    productToDelete = null;
    document.getElementById("deleteConfirmModal").style.display = "none";
}

function clearAddForm() {
    document.getElementById("pcode").value = "";
    document.getElementById("pname").value = "";
    document.getElementById("pbrand").value = "";
    document.getElementById("pcategory").value = "";
}

async function openEditModal(productCode) {

    isEditMode = true;

    try {
        const res = await fetch(`/Product/GetByCode/${productCode}`);
        const result = await res.json();

        if (result.statusCode !== 200) {
            console.error("GetByCode API Error:", result.error);
            return;
        }

        const p = result.data;

        // populate fields
        document.getElementById("pcode").value = p.product_code;
        document.getElementById("pname").value = p.product_name;
        document.getElementById("pbrand").value = p.brand;
        document.getElementById("pcategory").value = p.category;

        document.getElementById("pcode").disabled = true; // prevent editing PK

        document.getElementById("addProductModal").style.display = "flex";

    } catch (err) {
        console.error("Edit load error:", err);
    }
}

async function saveProduct() {
    const code = document.getElementById("pcode").value;
    const name = document.getElementById("pname").value;
    const brand = document.getElementById("pbrand").value;
    const category = document.getElementById("pcategory").value;

    const payload = {
        product_code: code,
        product_name: name,
        brand: brand,
        category: category
    };

    try {

        let url = "";

        if (isEditMode) {
            // EDIT MODE
            url = "/Product/Update";  
        } else {
            // ADD MODE
            url = "/Product/Add";      
        }

        const response = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(payload)
        });

        const result = await response.json();
        console.log("Add Response:", result);

        closeAddModal();

        loadProducts();

    } catch (err) {
        console.error("Add Error:", err);
    }
}

async function deleteProduct() {

    try {
        const res = await fetch(`/Product/Delete/${productToDelete}`);
        const result = await res.json();

        if (result.statusCode !== 200) {
            console.error("Delete API Error:", result.error);
            return;
        }

        loadProducts();
        closeDeleteConfirmationModal();

    } catch (err) {
        console.error("Delete load error:", err);
    }
}

async function loadProducts() {
    try {
        const response = await fetch("https://localhost:7287/Product/GetAll");

        const data = await response.json();

        if (data.statusCode !== 200) {
            console.error("API Error:", data.error);
            return;
        }

        const list = data.data;
        const tbody = document.getElementById("productsTableBody");

        tbody.innerHTML = "";

        list.forEach(product => {
            const row = `
                <tr>
                    <td>${product.product_code}</td>
                    <td>${product.product_name}</td>
                    <td>${product.brand}</td>
                    <td>${product.category}</td>
                    <td>
                        <button onclick="openEditModal('${product.product_code}')">
                            Edit
                        </button>
                        <button onclick="openDeleteConfirmationModal('${product.product_code}')" style="background: red;">
                            Delete
                        </button>
                    </td>
                </tr>
            `;
            tbody.innerHTML += row;
        });

    } catch (err) {
        console.error("Fetch Error:", err);
    }
}