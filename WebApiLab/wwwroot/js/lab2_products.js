const uri = 'api/products';
let products = [];

function getProducts() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayProducts(data))
        .catch(error => console.error('Unable to get products.', error));
}


function addProduct() {
    const addNameTextbox = document.getElementById('add-name');
    const addPriceTextbox = document.getElementById('add-price');
    const addCategoryTextbox = document.getElementById('add-cat');
    const category = {
        name: addCategoryTextbox.value.trim(),
	}
    const product = {
        name: addNameTextbox.value.trim(),
        price: parseInt(addPriceTextbox.value.trim()),
        category: category
    };
    console.log(product);
    const prodJsn = JSON.stringify(product);
    console.log(prodJsn);
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: prodJsn
    })
        .then(response => response.json())
        .then(() => {
            getProducts();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add product.', error));
}

function deleteProduct(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getProducts())
        .catch(error => console.error('Unable to delete product.', error));
}

function displayEditForm(id) {
    const product = products.find(prod => prod.id === id);

    document.getElementById('edit-id').value = product.id;
    document.getElementById('edit-name').value = product.name;
    document.getElementById('edit-price').value = product.price;
    ocument.getElementById('edit-cat').value = product.category.name;
    document.getElementById('editForm').style.display = 'block';
}

function updateProduct() {
    const prodId = document.getElementById('edit-id').value;

    const product = {
        id: parseInt(prodId, 10),
        name: document.getElementById('edit-name').value.trim(),
        price: parseInt(document.getElementById('edit-price').value.trim()),
    };
    console.log(product);
    fetch(`${uri}/${prodId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(product)
    })
        .then(() => getProducts())
        .catch(error => console.error('Unable to update product.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}




function _displayProducts(data) {
    const tBody = document.getElementById('products');
    tBody.innerHTML = '';


    const button = document.createElement('button');

    data.forEach(product => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${product.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteProduct(${product.id})`);

        let tr = tBody.insertRow();

     //   console.log(product);

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(product.name);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNode1 = document.createTextNode(product.price);
        td2.appendChild(textNode1);


        let td3 = tr.insertCell(2);
        let textNode2 = document.createTextNode(product.category);
        if (product.category != null)
            textNode2 = document.createTextNode(product.category.name);

        td3.appendChild(textNode2);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });

    products = data;
}
