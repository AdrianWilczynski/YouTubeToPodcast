//@ts-check
document.getElementById('copyButton')
    .addEventListener('click', function () {
        var copyTarget = document.getElementById('copyTarget');

        if (copyTarget instanceof HTMLInputElement) {
            copyTarget.hidden = false;

            copyTarget.select();
            document.execCommand('copy');
            copyTarget.blur();

            copyTarget.hidden = true;
        }
    });
