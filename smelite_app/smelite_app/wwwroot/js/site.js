$("#add-offer").on("click", function () {
    var index = $("#offerings .offering").length;
    var template = `<div class='offering border p-2 mb-2'>
        <input name='Offerings[${index}].LocationName' class='form-control mb-1' placeholder='Location' maxlength='300' required />
        <input name='Offerings[${index}].SessionsCount' type='number' min='0' max='20' class='form-control mb-1' placeholder='Sessions' required />
        <input name='Offerings[${index}].PackageLabel' class='form-control mb-1' placeholder='Label' maxlength='100' />
        <input name='Offerings[${index}].Price' class='form-control' placeholder='Price' type='number' min='0' max='100000' step='0.01' required />
        <button type='button' class='btn btn-sm btn-danger remove-offer mt-1'>Remove</button>
    </div>`;
    $("#offerings").append(template);
});

// Работи за всички (и за вече добавени, и за динамични)
$(document).on("click", ".remove-offer", function () {
    $(this).closest('.offering').remove();
});

if (window.React && window.ReactDOM && window['framer-motion']) {
    const { motion } = window['framer-motion'];
    const e = React.createElement;

    function AnimatedFlower() {
        return e(
            motion.svg,
            { width: 80, height: 80, viewBox: "0 0 80 80", style: { display: 'block', margin: 'auto' } },
            e(
                motion.circle,
                {
                    cx: 40,
                    cy: 40,
                    r: 18,
                    fill: "#C9A04A",
                    animate: { scale: [1, 1.2, 1] },
                    transition: { repeat: Infinity, duration: 2 }
                }
            ),
            // можеш да добавиш още венчелистчета
        );
    }

    ReactDOM.render(
        e(AnimatedFlower, {}),
        document.getElementById('framer-hero')
    );
}
