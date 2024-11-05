// Normally you can do  this with onclick but sometimes you want to trigger it in code
function CloseModal(id) {
    let element = document.querySelector(`#${id}`);
    element.close();
}

