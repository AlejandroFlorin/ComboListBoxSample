
function handleTextBoxOnKeyPress(maxListItemLength, ibtnAddConfirmID, evt) {

    //Cross-browser compatible event getter
	evt = (evt) ? evt : ((window.event) ? window.event : null);

    //Assumes ibtnAddConfirmID is valid
	var ibtnAddConfirm = document.getElementById(ibtnAddConfirmID);
	var typedCharCode = getTypedCharCode(evt);
	if (typedCharCode == 13) {
	    cancelEvent(evt);
	    ibtnAddConfirm.click();
	}

}

//Cross-browser compatible event canceller
function cancelEvent(evt) {
    try { evt.cancelBubble = true; } catch (e) { }
    try { evt.returnValue = false; } catch (e) { }
    try { evt.stopPropagation(); } catch (e) { }
    try { evt.preventDefault(); } catch (e) { }
}

//Cross-browser compatible character code getter
function getTypedCharCode(evt) {
    return evt.charCode ? evt.charCode : ((evt.which) ? evt.which : evt.keyCode);
}