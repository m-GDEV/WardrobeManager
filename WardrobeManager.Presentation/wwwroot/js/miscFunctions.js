// Normally you can do  this with onclick but sometimes you want to trigger it in code
function CloseModal(id) {
    let element = document.querySelector(`#${id}`);
    element.close();
}

function UpdateQueryParams(key, value) {
    // javascript is weird so i can't define what type the 'data' is 
    // for future reference, i expect the data to be a json object with 1-level of depth
    const params = new URLSearchParams(window.location.search);

    console.log(value)
    params.set(key, value)
    
    // updates/adds to query params, does not remove old ones
    history.replaceState(null, null, `${window.location.pathname}?${params.toString()}`);
}

