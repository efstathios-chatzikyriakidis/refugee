angular.module('refugeeApp')
.directive('d3graph', ['d3Service','$rootScope','growl','httpCaller', function(d3Service,$rootScope,growl,httpCaller) {
    return {
      restrict: 'EA',
      link: function(scope, element, attrs) {
        $rootScope.$on('updateGraph', function(event,path) {
        d3Service.d3().then(function(d3) {
            var width = 800, height = 800;
              // force layout setup
              var force = d3.layout.force()
                      .charge(-200).linkDistance(30).size([width, height]);
            
              //Remove previous svg's (if exist)
              d3.select("svg").remove();
            
              // setup svg div
              var svg = d3.select("#graph").append("svg")
                      .attr("width", "100%").attr("height", "100%")
                      .attr("pointer-events", "all");

              var address = httpCaller.getAddress();
            
              // load graph (nodes,links) json from /graph endpoint
              d3.json("http://"+httpCaller.getAddress()+"/Refugee.Server/api/refugees/"+path, function(error, graph) {
            if (error) return;

                  var nodeById = d3.map();
                
                  if(graph.Links.length==0)
                      growl.warning("This refugee has no relation yet", {ttl: 5000});
                  
                  graph.Nodes.forEach(function(node) {
                    nodeById.set(node.id, node);
                  });

                  graph.Links.forEach(function(link) {
                    link.source = nodeById.get(link.sourceId);
                    link.target = nodeById.get(link.targetId);
                  });

                  force
                      .nodes(graph.Nodes)
                      .links(graph.Links)
                      .start();

                  // render relationships as lines
                  var link = svg.selectAll(".link")
                          .data(graph.Links).enter()
                          .append("line").attr("class", "link");

                  // render nodes as circles, css-class from label
                  var node = svg.selectAll(".node")
                          .data(graph.Nodes).enter();
                  var gNode = node.append("g");
                  var circleNode = gNode.append("circle");
                  gNode.append("title")
                      .attr("x", function(d) { return d.x; })
                      .attr("y", function(d) { return d.y; })
                        .attr("text-anchor","middle")
                        .text(function (d) { return d.name; });
                  var fullNode = circleNode
                          .attr("class", function (d) { 
                              if(d.type==0)
                                  return "node Refugee";
                              else
                                  return "node Hotspot"; })
                        //Make hotspot node bigger than refugee
                          .attr("r", function(d){return d.type==0?12:19;})
                          .call(force.drag);
                 
                          

                  // force feed algo ticks for coordinate computation
                  force.on("tick", function(e) {
                      link.attr("x1", function(d) { return d.source.x; })
                              .attr("y1", function(d) { return d.source.y; })
                              .attr("x2", function(d) { return d.target.x; })
                              .attr("y2", function(d) { return d.target.y; });

                      fullNode.attr("cx", function(d) { return d.x; })
                              .attr("cy", function(d) { return d.y; });
                      
                      /*node.attr("transform", function(d) { 
                        //TODO move these constants to the header section
                        //center the center (root) node when graph is cooling down
                        if(d.index==0){
                            damper = 0.1;
                            d.x = d.x + (width/2 - d.x) * (damper + 0.71) * e.alpha;
                            d.y = d.y + (height/2 - d.y) * (damper + 0.71) * e.alpha;
                        }
                        //start is initiated when importing nodes from XML
                        if(d.start === true){
                            d.x = width/2;
                            d.y = height/2;
                            d.start = false;
                        }

                        r = d.name.length;
                        //these setting are used for bounding box, see [http://blockses.appspot.com/1129492][1]
                        d.x = Math.max(r, Math.min(width - r, d.x));
                        d.y = Math.max(r, Math.min(height - r, d.y));

                            return "translate("+d.x+","+d.y+")";            
                      });*/
                      
                  });
                  
                  growl.success("Graph succesfully fetched", {ttl: 2000});
              });
            },function(error){
                growl.error("Error while fetching graph", {ttl: 5000});
            });
        });
}}}]);
