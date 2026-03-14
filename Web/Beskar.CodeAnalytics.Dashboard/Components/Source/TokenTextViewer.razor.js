
window.scrollToToken = function(index) 
{
    const element = document.querySelector(`[data-index="${index}"]`);
    if (element) 
    {
        element.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }
}

window.scrollToLine = function(lineNumber) 
{
    const elements = document.querySelectorAll('.line-number');
    
    for (let el of elements) 
    {
        if (el.textContent.trim() === lineNumber.toString()) 
        {
            el.scrollIntoView({behavior: 'smooth', block: 'center'});
            break;
        }
    }
}
