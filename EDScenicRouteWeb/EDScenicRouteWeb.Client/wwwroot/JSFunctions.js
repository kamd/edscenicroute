window.extraJsFunctions = {
    selectid: function (id) {
        window.getSelection().selectAllChildren(document.getElementById(id));
    }
};