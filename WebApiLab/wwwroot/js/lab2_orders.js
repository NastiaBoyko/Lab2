const uri = 'api/orders';
let orders = [];

function getOrders() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayOrders(data))
        .catch(error => console.error('Unable to get orders.', error));
}


function addOrder() {
    const addNameTextbox = document.getElementById('add-name');
 
    const order = {
        name: addNameTextbox.value.trim(),
    };
    const prodJsn = JSON.stringify(order);
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
            getOrders();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add order.', error));
}

function deleteOrder(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getOrders())
        .catch(error => console.error('Unable to delete order.', error));
}

function displayEditForm(id) {
    const order = orders.find(ord => ord.id === id);

    document.getElementById('edit-id').value = order.id;
    document.getElementById('edit-name').value = order.name;
    document.getElementById('editForm').style.display = 'block';
}

function updateOrder() {
    const ordId = document.getElementById('edit-id').value;

    const order = {
        id: parseInt(ordId, 10),
        name: document.getElementById('edit-name').value.trim(),
    };
    console.log(order);
    fetch(`${uri}/${ordId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(order)
    })
        .then(() => getOrders())
        .catch(error => console.error('Unable to update order.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}


function addProduct(ordId) {
    let input = document.getElementById("add-product" + ordId);
    let prodName = input.value.trim();

    const product = {
        name: prodName,
    };
    fetch('api/orders/' + ordId +'/products/', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(product)
    })
        .then(() => {
            getOrders();
            input.value = '';
        }).catch(error => console.error('Unable to add product.', error));
}

function deleteProduct(ordId, prodId) {
    let input = document.getElementById("add-product" + ordId);
    let prodName = input.value.trim();

    fetch('api/orders/'+ordId+'/products/'+ prodId, {
        method: 'DELETE',
    })
        .then(() => {
            getOrders();
        }).catch(error => console.error('Unable to add product.', error));
}




function _displayOrders(data) {
    const tBody = document.getElementById('orders');
    tBody.innerHTML = '';


    const button = document.createElement('button');

    data.forEach(order => {
        var input = document.createElement("input");
        input.type = "text";
        input.id = "add-product" + order.id;

        let addProductButton = button.cloneNode(false);
        addProductButton.innerText = 'AddProduct';
        addProductButton.setAttribute('onclick', `addProduct(${order.id}) `);

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${order.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteOrder(${order.id})`);



        let tr = tBody.insertRow();

           console.log(order);

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(order.name);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        td2.appendChild(input);


        let td3 = tr.insertCell(2);
        td3.appendChild(addProductButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);



        for (var i = 0; i < order.products.length; i++) {
            let prod_tr = tBody.insertRow();
            let product = order.products[i].product;
            console.log(product);
            let prod_td = prod_tr.insertCell(0);
            let prod_count = document.createTextNode(i+1);
            prod_td.appendChild(prod_count);


            let prod_td1 = prod_tr.insertCell(1);
            let prod_textNode = document.createTextNode(product.name);
            prod_td1.appendChild(prod_textNode);

            let prod_td2 = prod_tr.insertCell(2);
            let prod_textNode1 = document.createTextNode(product.price);
            prod_td2.appendChild(prod_textNode1);


            let prod_td3 = prod_tr.insertCell(3);
            let prod_textNode2 = document.createTextNode(product.category);
            if (product.category != null)
                prod_textNode2 = document.createTextNode(product.category.name);

            prod_td3.appendChild(prod_textNode2);


            let prod_deleteButton = document.createElement('button');
             prod_deleteButton.innerText = 'Delete from order';
             prod_deleteButton.setAttribute('onclick', `deleteProduct(${order.id}, ${product.id})`);

            let prod_td5 = prod_tr.insertCell(4);
            prod_td5.appendChild( prod_deleteButton);

		}
    });

    orders = data;
}
