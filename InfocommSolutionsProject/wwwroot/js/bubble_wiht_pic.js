const $pause = false;
// this drives the number of bubbles occurring
const bubbleImages = [
   'https://drive.google.com/file/d/1WO2VvTVDXQDFaKAwxiiteNYESkn7IdIb/view?usp=sharing',
   'https://placeimg.com/205/200/animals',
   'https://placeimg.com/202/200/animals',
   'https://placeimg.com/201/200/animals',
   'https://placeimg.com/204/200/animals',
   'https://placeimg.com/206/200/animals',
   'https://placeimg.com/208/200/animals',
   'https://placeimg.com/400/400/animals'
];

// x, y cordinates could be proven useful for animating the modal from this location
function animateBubbleModal(x, y, bubble) {
   
   const $modal = document.getElementById('bubble-modal');
   $modal.classList.add('active');
   
   // modal background
   const $modalbg = document.createElement('div');
   $modalbg.classList.add('bubble-modal-bg');
   document.body.appendChild($modalbg);
}

function createBubble(imageIndex) {
   // create bubble graphic
   const $bubble = document.createElement('div');
   
   // add class and background image
   $bubble.classList.add('bubble');
   $bubble.style.backgroundImage = 'url(' + bubbleImages[imageIndex] + ')';
   
   // wrap in a larger div so bubbles are easy to pop while moving
   const $boundingBox = document.createElement('div');
   $boundingBox.classList.add('bubble-wrap');
   
   // start at 10vw and max of 90vw
   $boundingBox.style.left = (10 + (Math.random() * 80))  + 'vw';

   // add bubble to bounding box and event listener
   $boundingBox.appendChild($bubble);
   $boundingBox.addEventListener('click', destroyBubble($boundingBox));
   
   // attach to doc and return
   document.body.appendChild($boundingBox);
   return $boundingBox;
}

function createExplosion(x, y) {
    // create explosion at the coordinates
    const $explosion = document.createElement('div');
    $explosion.classList.add('explosion');
    
    // set the cordinates of the "explosion" useful for generating location for graphic
    $explosion.style.left = x + 'px';
    $explosion.style.top = y + 'px';
   
    // set text - again could put anything in here like the start of an animation 
    $explosion.innerHTML = "Clicked";
    
    // add element to dom
    document.body.appendChild($explosion);
   
    // animate cartoon pop on words
    just.animate({
       targets: $explosion,
       to: 600,
       fill: 'forwards',
       easing: 'ease-out',
       css: [
          { scale: 1 },
          { offset: 0.2, scale: 1.4, opacity: 1 },
          { scale: .7, opacity: 0 }
       ]
    })
    .on('finish', () => document.body.removeChild($explosion));
}

function destroyBubble($bubble) {
   return () => {
      // get the bubble bounding box and location
      const rect = $bubble.getBoundingClientRect();
      const centerX = (rect.right - rect.left) * .45 + rect.left;
      const centerY = (rect.bottom - rect.top) * .45 + rect.top;
      
      // add animation for deleting bubble
      createExplosion(centerX, centerY);
      
      // create modal here
      //animateBubbleModal(centerX, centerY, $bubble);

      // remove bubble
      $bubble.style.display = 'none';
   }
}

function generateBubbles(length) {
   const targets = []
   for (let i = 0; i < length; i++) {
      targets.push(createBubble(i));
   }
   return targets;
}

function animateBubbles() {
   const bubbles = generateBubbles(bubbleImages.length);
   const player = just.animate({
      targets: bubbles,
      to: '5s',
      easing: 'ease-in',
      css: {
         transform() {
            const endTranslateY = just.random(100, 110, 'vh', true);
            const startScale = just.random(80, 100, null, true);
            const endScale = just.random(40, 80, null, true);
            
            return [
               'translateY(0) scale(0.' + startScale + ')', 
               'translatey(-' + endTranslateY + ') scale(0.' + endScale + ')'
            ];
         }
      },
      delay() {
         return just.random(0, 10000);
      }
   })
   .on('finish', () => {
      bubbles.forEach(bubble => {
         document.body.removeChild(bubble);
      });
   });
   
   return player;
}

function startBubbles() {
   animateBubbles().on('finish', startBubbles);
}



startBubbles();
