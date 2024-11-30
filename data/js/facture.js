function openInvoice(prenom, nom, actes) {
    let width = 500;
    let height = 300;
    let left = 0;
    let top = 0;
    if (window.innerWidth) {
        left = (window.innerWidth - width) / 2;
        top = (window.innerHeight - height) / 2;
    } else {
        left = (document.body.clientWidth - width) / 2;
        top = (document.body.clientHeight - height) / 2;
    }
    
    const options = `menubar=yes, scrollbars=yes, top=${top}', left=${left}', width=${width}, height=${height}`;
    let factureWindow = window.open('', 'facture', options);
    
    let factureText = "Facture pour : " + prenom + " " + nom;
    factureWindow.document.write(factureText);
}